<template>
  <v-container fluid fill-height justify-center>
    <v-card>
      <PlayerInfo />
    </v-card>

    <v-alert v-if="wordleGame.gameOver" width="80%" :type="gameResult.type">
      {{ gameResult.text }}
      <v-btn class="ml-2" @click="resetGame"> Play Again? </v-btn>
    </v-alert>
    <v-row justify="center" class="my-16">
      <game-board :wordleGame="wordleGame" />
    </v-row>
    <v-row justify="center" class="my-16">
      <keyboard :wordleGame="wordleGame" />
    </v-row>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { WordsService } from '../scripts/wordsService'
import { GameState, WordleGame } from '../scripts/wordleGame'
import { Word } from '../scripts/word'
import KeyBoard from '../components/keyboard.vue'
import GameBoard from '../components/game-board.vue'
import PlayerInfo from '~/components/player-info.vue'
@Component({ components: { KeyBoard, GameBoard } })
export default class Game extends Vue {
  word: string = WordsService.getRandomWord()
  wordleGame = new WordleGame(this.word)
  gameCount = 1
  startTime: Date = new Date()
  finishTime: Date = new Date()
  totalTime = 0
  player: PlayerInfo = new PlayerInfo()

  resetGame() {
    this.word = WordsService.getRandomWord()
    this.wordleGame = new WordleGame(this.word)
    this.gameCount++
  }

  get gameResult() {
    this.finishTime = new Date()
    if (this.wordleGame.state === GameState.Won) {
      this.totalTime =
        (this.finishTime.getTime() - this.startTime.getTime()) / 1000

      return {
        type: 'success',
        text: `Congrats ${this.player.getName()}! You won in ${
          this.totalTime
        } seconds!`,
      }
    }
    if (this.wordleGame.state === GameState.Lost) {
      return { type: 'error', text: `You lost... :( The word was ${this.word}` }
    }
    return { type: '', text: '' }
  }

  getLetter(row: number, index: number) {
    const word: Word = this.wordleGame.words[row - 1]
    if (word !== undefined) {
      return word.letters[index - 1]?.char ?? ''
    }
    return ''
  }
}
</script>
