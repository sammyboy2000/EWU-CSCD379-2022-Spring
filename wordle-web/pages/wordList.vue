<template>
  <v-container fluid fill-height justify-center>
    <v-card width="50%">
      <v-card-title class="display-3 justify-center"> Word List </v-card-title>
      <v-card-text class="text-center">
        {{ title }}
        <v-text-field v-model="searchterm" maxlength="5" dense filled>
        </v-text-field>
      </v-card-text>
      <v-card-text>
        <v-simple-table>
          <thead>
            <tr>
              <th>Word</th>
              <th style="text-align: center">Options</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="word in words" :key="word">
              <td>{{ word }}</td>
              <td style="text-align: center">
                <!--{{ player.averageSecondsPerGame }} -->
              </td>
            </tr>
          </tbody>
        </v-simple-table>
        <v-row>
          <v-col>
            <v-pagination
              v-model.number="page"
              :length="pages"
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

      <!-- <v-card-actions>
        <v-btn color="primary" @click="getAllPlayers"> Get All Players </v-btn>
        <v-spacer />
        <v-btn color="primary" @click="getTop10Players">
          Get Top 10 Players
        </v-btn>
      </v-card-actions> -->
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue, Watch } from 'vue-property-decorator'

@Component({})
export default class wordlist extends Vue {
  players: any = []
  title: string = ''
  searchterm: string = ''
  words: any = []
  page: number = 1
  perPage: number = 10
  pages: number = 1

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

  update() {
    if (this.searchterm === '') {
      this.getAllWords()
    } else {
      this.getSearch()
    }
  }

  created() {
    this.getAllWords()
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
        `/api/Word/GetWordListPage?partialWord=${this.searchterm}&page=${
          this.page - 1
        }&count=${this.perPage}`
      )
      .then((response) => {
        this.words = response.data
      })
  }
}
</script>
