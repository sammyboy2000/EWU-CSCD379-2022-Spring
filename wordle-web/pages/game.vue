<template>
  <v-container fluid fill-height justify-center>
    <v-alert
      v-if="wordleGame.gameOver"
      width="80%"
      justify-center
      :type="gameResult.type"
    >
      {{ gameResult.text }}
      <v-btn class="ml-2" @click="resetGame"> Play Again? </v-btn>
      <span v-if="player.getName() == 'Guest'"
        >Click name to change it or submit score?</span
      >
      <v-btn v-if="gameResult.type == 'success'" @click="postScore()"
        >Submit Score</v-btn
      >
    </v-alert>
    <PlayerInfo />
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
  startTime: Date = new Date()
  finishTime: Date = new Date()
  totalTime = 0
  player: PlayerInfo = new PlayerInfo()

  resetGame() {
    this.word = WordsService.getRandomWord()
    this.wordleGame = new WordleGame(this.word)
    this.startTime = new Date()
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

  postScore() {
    this.$axios
      .post('/api/LeaderBoard', {
        score: this.wordleGame.words.length,
        name: `${this.player.getName()}`,
        seconds: this.totalTime.toFixed(),
      })
      .then(function (response) {
        console.log(response)
      })
      .catch(function (error) {
        console.log(error)
      })
    this.$axios
      .post('/api/ScoreStats', {
        score: this.wordleGame.words.length,
        seconds: this.totalTime.toFixed(),
      })
      .then(function (response) {
        console.log(response)
      })
      .catch(function (error) {
        console.log(error)
      })
    this.resetGame()
  }
}
</script>
