<template>
  <div class>
    <v-container @click="toggleDialog">
      How To Play <v-icon>mdi-cog</v-icon>
    </v-container>

    <v-dialog v-model="dialog" width="450">
      <v-card>
        <v-container>
          <v-card-title>How To Play</v-card-title>
          <v-card-text justify="center">
            Guess a word!
            <v-img
              src="/wordexample.png"
              max-height="50"
              max-width="250"
            ></v-img>
            Wrong letters will be greyed out.
            <v-img
              src="/letterwrong.png"
              max-height="50"
              max-width="50"
            ></v-img>
            Correct letters in the wrong spot will change to a different color.
            <v-img src="/letterwarn.png" max-height="50" max-width="50"></v-img>
            Correct letters in the correct position will change to your themes
            main color.
            <v-img
              src="/lettercorrect.png"
              max-height="50"
              max-width="50"
            ></v-img>
            Use question marks to fill in letters that you don't know. You can
            use the hint function at the bottom of the keyboard to see the
            possible words.
            <v-img src="/wordguess.png" max-height="50" max-width="250"></v-img>
            Use this information to guess the correct word!
            <v-img
              src="/wordcorrect.png"
              max-height="50"
              max-width="250"
            ></v-img>
          </v-card-text>
        </v-container>
      </v-card>
    </v-dialog>
  </div>
</template>
<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'

@Component({})
export default class SettingsDialog extends Vue {
  dialog = false

  toggleDialog() {
    this.dialog = !this.dialog
  }
}
</script>
