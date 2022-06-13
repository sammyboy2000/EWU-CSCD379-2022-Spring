<template>
  <v-container fluid fill-height justify-center>
    <v-card width="50%" class="text-center">
      <v-alert v-if="authorized" type="primary"
        >Welcome, Master Of The Universe</v-alert
      >
      <v-card-title class="display-3 justify-center"> Word List </v-card-title>
      <v-card-text class="text-center">
        {{ title }}
        <v-text-field v-model="searchterm" maxlength="5" dense filled>
        </v-text-field>
        <v-card-text v-if="authorized" class="text-center">
          Word not in this list?

          <v-btn @click="addWord(searchterm)"> Add Word </v-btn>
        </v-card-text>
      </v-card-text>
      <v-card-text>
        <v-simple-table>
          <thead>
            <tr>
              <th>Word</th>
              <th style="text-align: center">Is this word common?</th>
              <th v-if="authorized" style="text-align: center">Delete?</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="word in words" :key="word">
              <td>{{ word }}</td>
              <td style="text-align: center">
                <v-btn icon @click="voteUncommon(word)">
                  <v-icon>mdi-thumb-down</v-icon></v-btn
                >
                <v-btn icon @click="voteCommon(word)">
                  <v-icon>mdi-thumb-up</v-icon></v-btn
                >
              </td>
              <td v-if="authorized" style="text-align: center">
                <v-btn icon @click="deleteWord(word)">
                  <v-icon>mdi-delete</v-icon></v-btn
                >
              </td>
            </tr>
          </tbody>
        </v-simple-table>
        <v-row>
          <v-col>
            <v-pagination
              v-model.number="page"
              :length="pages"
              total-visible="10"
              @input="update"
            ></v-pagination>
          </v-col>
        </v-row>
        <v-spacer />
        <v-row>
          <v-col cols="6">
            <v-text-field
              v-model.number="perPage"
              maxlength="4"
              type="number"
              label="Words per Page"
              dense
              filled
            ></v-text-field>
          </v-col>
          <v-col cols="6">
            <v-text-field
              v-model.number="page"
              maxlength="7"
              type="number"
              label="Current Page"
              dense
              filled
            ></v-text-field>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator'
import { JWT } from '~/scripts/jwt'

@Component({})
export default class wordlist extends Vue {
  players: any = []
  title: string = ''
  searchterm: string = ''
  words: any = []
  page: number = 1
  perPage: number = 10
  pages: number = 1
  token: JWT = new JWT()
  roles: string[] = ['Not', 'Authorized']
  authorized: boolean = false

  @Watch('searchterm')
  onSearchChanged() {
    this.title = `Search for words that start with ${this.searchterm}`
    this.update()
  }

  @Watch('perPage')
  onPerPageChanged() {
    this.update()
  }

  @Watch('page')
  onPageChanged() {
    this.update()
  }

  @Watch('pages')
  onPagesChanged() {
    this.page = 1
  }

  update() {
    if (this.searchterm === '') {
      this.getAllWords()
    } else {
      this.getSearch()
    }
  }

  created() {
    this.checkLoggedIn()
    this.getAllWords()
    JWT.setToken(localStorage.getItem('token'), this.$axios)
    this.roles = JWT.tokenData.roles
    if (this.roles != null) {
      if (
        this.roles.includes('MasterOfTheUniverse') &&
        JWT.tokenData.age >= 21
      ) {
        this.authorized = true
      }
    }
  }

  checkLoggedIn() {
    if (
      localStorage.getItem('token') === null ||
      localStorage.getItem('token')!.length < 5
    ) {
      this.$axios
        .post('Token/GetToken', {
          username: 'Guest@intellitect.com',
          password: 'P@ssw0rd123',
        })
        .then((result) => {
          JWT.setToken(result.data.token, this.$axios)
        })
    }
  }

  getAllWords() {
    this.title = 'Search for words that start with:'
    this.$axios
      .get('/api/Word/GetAllNumberOfPages?count=' + this.perPage)
      .then((response) => {
        this.pages = response.data
      })
    this.$axios
      .get(
        `/api/Word/GetAllWordListPage?page=${this.page - 1}&count=${
          this.perPage
        }`
      )
      .then((response) => {
        this.words = response.data
      })
  }

  getSearch() {
    this.$axios
      .get(
        `/api/Word/GetNumberOfPages?partialWord=${this.searchterm}&count=${this.perPage}`
      )
      .then((response) => {
        this.pages = response.data
      })
    this.$axios
      .get(
        `/api/Word/GetWordListPage?partialWord=${this.searchterm}&page=${
          this.page - 1
        }&count=${this.perPage}`
      )
      .then((response) => {
        this.words = response.data
      })
  }

  voteCommon(word: string) {
    if (word.length === 5) {
      this.$axios
        .post(`/api/Word/MakeWordCommon?word=${word}`)
        .then((response) => {
          if (!response.data) {
            alert('Vote Failed')
          } else alert('Vote Successful')
        })
    }
  }

  voteUncommon(word: string) {
    if (word.length === 5) {
      this.$axios
        .post(`/api/Word/MakeWordUncommon?word=${word}`)
        .then((response) => {
          if (!response.data) {
            alert('Vote Failed')
          } else alert('Vote Successful')
        })
    }
  }

  deleteWord(word: string) {
    if (word.length === 5) {
      this.$axios.post(`/api/Word/DeleteWord?word=${word}`).then((response) => {
        if (response.data) {
          alert('Delete Successful')
          window.location.reload()
        } else alert('Delete Failed')
      })
    } else alert('Delete Failed')
  }

  addWord(word: string) {
    if (word.length === 5) {
      this.$axios.post(`/api/Word/AddWord?word=${word}`).then((response) => {
        if (response.data) {
          alert('Add Successful')
          window.location.reload()
        } else alert('Add Failed')
      })
    } else alert('Add Failed')
  }
}
</script>
