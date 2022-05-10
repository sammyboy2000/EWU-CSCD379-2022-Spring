<template>
  <v-container fluid fill-height justify-center>
    <v-tooltip bottom>
      <template #activator="{ on, attrs }">
        <v-btn color="primary" nuxt to="/" fab v-bind="attrs" v-on="on">
          <v-icon> mdi-home </v-icon>
        </v-btn>
      </template>
      <span> Go Home </span>
    </v-tooltip>
    <v-card>
      <v-card-title>
        <h1 class="display-1">Leader Board</h1>
      </v-card-title>
      <v-card-text>
        <v-simple-table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Games</th>
              <th>Avg. Score</th>
              <th>Avg. Seconds</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(player, index) in players" :key="index">
              <td>{{ player.name }}</td>
              <td>{{ player.gameCount }}</td>
              <td>{{ player.averageAttempts }}</td>
              <td>{{ player.averageSeconds }}</td>
            </tr>
          </tbody>
        </v-simple-table>
      </v-card-text>
      <v-card-actions>
        <v-btn color="primary" @click="refreshPlayers"> Refresh </v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component({})
export default class LeaderBoard extends Vue {
  players: any = []

  refreshPlayers() {
    this.$axios.get('/api/LeaderBoard').then((response) => {
      this.players = response.data
    })
  }
}
</script>
