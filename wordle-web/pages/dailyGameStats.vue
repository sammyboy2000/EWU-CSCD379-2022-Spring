<template>
  <v-container fluid fill-height justify-center>
    <v-card>
      <v-card-title class="display-3 justify-center">
        Daily Games
      </v-card-title>
      <v-card-text class="text-center">
        {{ title }}
      </v-card-text>
      <v-card-text>
        <v-simple-table>
          <thead>
            <tr>
              <th>Date</th>
              <th style="text-align: center"># Plays</th>
              <th style="text-align: center">Avg. Score</th>
              <th style="text-align: center">Avg. Seconds</th>
              <th style="text-align: center">Played?</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(dateWord, date) in dateWords" :key="date">
              <td>{{ dateWord.date }}</td>
              <td style="text-align: center">{{ dateWord.numPlays }}</td>
              <td style="text-align: center">
                {{ dateWord.averageScore }}
              </td>
              <td style="text-align: center">
                {{ dateWord.averageTime }}
              </td>
              <td style="text-align: center">
                {{ dateWord.hasPlayed }}
              </td>
            </tr>
          </tbody>
        </v-simple-table>
      </v-card-text>

      <v-card-actions>
        <v-btn color="primary" @click="getLast10DateWords">
          Get Stats From Last 10 Daily Words
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component({})
export default class leaderboard extends Vue {
  dateWords: any = []
  title: string = ''

  created() {
    this.getLast10DateWords()
  }

  // Still need to pass in the Player Guid!!!
  getLast10DateWords() {
    this.title = 'Last 10 Daily Words'
    this.$axios.get('/api/DateWord').then((response) => {
      this.dateWords = response.data
    })
  }
}
</script>
