<template>
  <v-menu max-height="360" :offset-x="true" :close-on-content-click="true">
    <template #activator="{ on, attrs }">
      <v-btn
        :disabled="helpIsDisabled"
        color="accent"
        dark
        v-bind="attrs"
        v-on="on"
      >
        {{ numberOfValidWords }}
      </v-btn>
    </template>
    <v-list>
      <v-list-item v-for="(word, index) in validWordList" :key="index">
        <v-list-item-group>
          <v-list-item @click="setWord(word)">
            <v-list-item-title v-text="word"></v-list-item-title>
          </v-list-item>
        </v-list-item-group>
      </v-list-item>
    </v-list>
  </v-menu>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'
import { Word } from '../scripts/word'
import { WordsService } from '../scripts/wordsService'
import KeyBoard from './keyboard.vue'

@Component
export default class HelpButton extends Vue {
  @Prop({ required: true })
  keyboard!: KeyBoard

  setWord(word: string) {
    this.keyboard.setWord(word)
  }

  get helpIsDisabled(): boolean {
    if (
      this.keyboard.wordleGame.currentWord.length === 0 ||
      this.keyboard.wordleGame.gameOver ||
      this.keyboard.numberOfValidWords === 0 ||
      (this.keyboard.wordleGame.currentWord.length === 5 &&
        this.keyboard.numberOfValidWords === 1)
    ) {
      return true
    }
    return false
  }

  get validWordList() {
    const word: Word = this.keyboard.wordleGame.currentWord
    if (word !== undefined) {
      if (word.length > 0) {
        return WordsService.validWords(word)
      }
      return undefined
    }
  }

  get numberOfValidWords() {
    if (this.validWordList !== undefined) {
      return this.validWordList.length
    }
    return 'Word List'
  }
}
</script>
