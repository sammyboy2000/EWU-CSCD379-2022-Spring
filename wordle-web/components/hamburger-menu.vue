<template>
  <v-card>
    <v-btn icon @click="toggleDialog">
      <v-icon color="secondary"> mdi-hamburger </v-icon>
    </v-btn>
    <v-dialog
    v-model="dialog"
    transition="scale-transition"
    max-width="64"
    >
      <v-btn
        to="/"
        rounded
        min-width="64"
        min-height="64"
        class="justify-center"
        @click="toggleDialog"
      >
        <v-icon x-large>mdi-home</v-icon>
      </v-btn>
      <v-btn
      to="/game"
      rounded
      min-width="64"
      min-height="64"
      class="justify-center"
      @click="toggleDialog">
        <v-icon x-large>mdi-file-word-box</v-icon>
      </v-btn>
      <v-btn
        to="/ScoreStats"
        rounded
        min-width="64"
        min-height="64"
        class="justify-center"
        @click="toggleDialog"
      >
        <v-icon x-large>mdi-format-list-numbered</v-icon>
      </v-btn>
      <v-btn
        to="/leaderBoard"
        rounded
        min-width="64"
        min-height="64"
        class="justify-center"
        @click="toggleDialog"
      >
        <v-icon x-large>mdi-trophy-award</v-icon>
      </v-btn>
      <v-btn
      to="/about"
      rounded
      min-width="64"
      min-height="64"
      class="justify-center"
      @click="toggleDialog">
        <v-icon x-large>mdi-information</v-icon>
      </v-btn>
    </v-dialog>
  </v-card>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
@Component({})
export default class HamburgerMenu extends Vue {
  dialog = false
  toggleDialog() {
    this.dialog = !this.dialog
  }
}
</script>
