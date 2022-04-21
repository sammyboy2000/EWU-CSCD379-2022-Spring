<template>
  <v-card class="my-5 pa-5">
    <v-row v-for="(charRow, i) in chars" :key="i" no-gutters justify="center">
      <v-col v-for="char in charRow" :key="char" cols="1">
        <v-container class="text-center">
          <v-btn
            :color="letterColor(char)"
            :disabled="wordleGame.gameOver"
            @click="setLetter(char)"
          >
            {{ char }}
          </v-btn>
        </v-container>
      </v-col>
    </v-row>
    <v-btn
      :disabled="wordleGame.gameOver"
      class="float-left"
      @click="guessWord"
    >
      Guess
    </v-btn>
    <v-btn
      :disabled="wordleGame.gameOver"
      class="float-right"
      @click="removeLetter"
    >
      <v-icon>mdi-backspace</v-icon>
    </v-btn>
    <v-container>
      <v-row
      align="center"
      justify="center">
        <help-button :keyboard="this" />
      </v-row>
    </v-container>
  </v-card>
</template>



<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator'
import { Letter, LetterStatus } from '../scripts/letter'
import { Word } from '../scripts/word'
import { WordleGame } from '../scripts/wordleGame'
import { WordsService } from '../scripts/wordsService'
import HelpButton from './help-button.vue'

@Component({components: {HelpButton}})
export default class KeyBoard extends Vue {
  @Prop({ required: true })
  wordleGame!: WordleGame

  chars = [
    ['q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p'],
    ['a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l'],
    ['z', 'x', 'c', 'v', 'b', 'n', 'm', '?'],
  ]

  get helpIsDisabled():boolean{
    if(
      this.wordleGame.currentWord.length === 0 ||
      this.wordleGame.gameOver ||
      this.numberOfValidWords === 0 ||
      this.wordleGame.currentWord.length === 5){
      return true
    }
    return false
  }

  setLetter(char: string) {
    this.wordleGame.currentWord.addLetter(char)
  }

  removeLetter() {
    this.wordleGame.currentWord.removeLetter()
  }

  setWord(word: string) {
    while (this.wordleGame.currentWord.length > 0) {
      this.removeLetter()
    }
    for (let i = 0; i < word.length; i++) {
      this.setLetter(word[i])
    }
  }

  guessWord() {
    if (
      this.wordleGame.currentWord.length ===
      this.wordleGame.currentWord.maxLetters
    ) {
      this.wordleGame.submitWord()
    }
  }

  letterColor(char: string): string {
    if (this.wordleGame.correctChars.includes(char)) {
      return Letter.getColorCode(LetterStatus.Correct)
    }
    if (this.wordleGame.wrongPlaceChars.includes(char)) {
      return Letter.getColorCode(LetterStatus.WrongPlace)
    }
    if (this.wordleGame.wrongChars.includes(char)) {
      return Letter.getColorCode(LetterStatus.Wrong)
    }

    return Letter.getColorCode(LetterStatus.Unknown)
  }

  get validWordList() {
    const word: Word = this.wordleGame.currentWord
    if (word !== undefined) {
      if(word.length > 0){
        return WordsService.validWords(word)
      }
      return undefined
    }
  }

  get numberOfValidWords() {
    if (this.validWordList !== undefined) {
      return this.validWordList.length
    }
    return "Word List"
  }
}
</script>
