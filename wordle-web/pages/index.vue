<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col cols="8">
        <v-card color="transparent" flat>
          <v-card-title class="justify-center">
            <NotWordleLogo />
          </v-card-title>
          <v-card color="transparent" flat>
            <v-card-actions class="justify-center">
              <v-btn nuxt to="/game" color="primary" x-large width="200px">
                Play
              </v-btn>
            </v-card-actions>
          </v-card>
          <v-card color="transparent" flat>
            <v-card-actions class="justify-center">
              <v-btn nuxt to="/dailyGame" color="primary" x-large width="200px">
                Play The Daily
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import Component from 'vue-class-component'


@Component
export default class IndexPage extends Vue {}
</script>

<!-- <v-btn color="info" nuxt to="/leaderboard">
            Leader Board
          </v-btn> -->

function getDay() { throw new Error('Function not implemented.') }

