<template>
  <v-container fluid fill-height>
    <v-container v-if="!isLoaded">
      <v-row justify="center">
        <v-card loading>
          <v-card-title class="justify-center">
            Game is loading...
          </v-card-title>
        </v-card>
      </v-row>
    </v-container>
    <v-container v-if="isLoaded">
      <v-row justify="left">
        <v-col cols="4">
          <v-card-text align="right">
            <v-icon>mdi-timer</v-icon>
            {{ displayTimer() }}
          </v-card-text></v-col
        >
        <v-col cols="5" class="d-flex flex-row-reverse">
          <v-dialog v-model="dialog" justify-end persistent max-width="600px">
            <template #activator="{ on, attrs }">
              <v-btn color="primary" dark v-bind="attrs" v-on="on">
                {{ playerName }}
              </v-btn>
            </template>
            <v-card>
              <v-card-text>
                <v-text-field
                  v-model="playerName"
                  type="text"
                  placeholder="Guest"
                ></v-text-field>
              </v-card-text>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="dialog = false">
                  Close
                </v-btn>
                <v-btn
                  color="blue darken-1"
                  text
                  @click=";(dialog = false), setUser(playerName)"
                >
                  Save
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-col>
      </v-row>
      <v-row v-if="!isSmall()">
        <v-col cols="2"></v-col>
        <v-col cols="8" class="mt-0 mb-0 pt-0 pb-0">
          <v-img
            src="logo.jpeg"
            class="center"
            style="max-width: 100%; height: auto"
          />
        </v-col>
      </v-row>
      <v-row justify="center" class="mt-10">
        <v-alert v-if="wordleGame.gameOver" width="80%" :type="gameResult.type">
          {{ gameResult.text }}
          <v-btn class="ml-2" @click="resetGame"> Play Again? </v-btn>
        </v-alert>
      </v-row>

      <v-row justify="center">
        <game-board v-if="!isSmall()" :wordleGame="wordleGame" />
        <smallGame-board v-if="isSmall()" :wordleGame="wordleGame" />
      </v-row>
      <v-row justify="center">
        <smallKeyboard v-if="isSmall()" :wordleGame="wordleGame" />
        <keyboard v-if="!isSmall()" :wordleGame="wordleGame" />
      </v-row>
    </v-container>
  </v-container>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { GameState, WordleGame } from '~/scripts/wordleGame'
import KeyBoard from '@/components/keyboard.vue'
import GameBoard from '@/components/game-board.vue'
import { Word } from '~/scripts/word'

@Component({ components: { KeyBoard, GameBoard } })
export default class DailyGame extends Vue {
  // ? need this for closing button
  dialog: boolean = false
  playerName: string = ''
  playerGuid: string = ''
  timeInSeconds: number = 0
  startTime: number = 0
  endTime: number = 0
  intervalID: any
  date: Date = new Date()
  word: string = ''
  wordleGame = new WordleGame(this.word)

  isLoaded: boolean = false

  isSmall() {
    return this.$vuetify.breakpoint.smAndDown
  }

  mounted() {
    setTimeout(() => {
      this.isLoaded = true
    }, 5000)
    this.retrieveGuid()
    this.retrieveUserName()
    setTimeout(() => {
      this.getDailyWord()
    }, 3000)
    setTimeout(() => this.startTimer(), 5000) // delay is for initialization
  }

  // This method is probably unneeded
  resetGame() {
    this.wordleGame = new WordleGame(this.word)
    this.timeInSeconds = 0
    this.startTimer()
  }

  getDailyWord() {
    this.$axios
      .post('/api/DateWord', {
        date: new Date(),
        playerGuid: this.playerGuid
      })
      .then((response) => {
        this.word = response.data.word
        this.wordleGame = new WordleGame(this.word)
      })
      .catch(function (error) {
        alert(error)
      })
  }

  get gameResult() {
    this.stopTimer()
    this.timeInSeconds = Math.floor(this.endTime - this.startTime)
    if (this.wordleGame.state === GameState.Won) {
      if (
        this.playerName.toLocaleLowerCase() !== 'guest' &&
        this.playerName !== ''
      ) {
        this.endGameSave()
      } else {
        this.dialog = true
      }
      return { type: 'success', text: 'You won! :^)' }
    }
    if (this.wordleGame.state === GameState.Lost) {
      return {
        type: 'error',
        text: `You lost... :^( The word was ${this.word}`,
      }
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

  retrieveUserName() {
    const userName = localStorage.getItem('userName')
    if (userName == null) {
      this.playerName = 'Guest'
    } else {
      this.playerName = userName
    }
  }

  retrieveGuid() {
    const guid = localStorage.getItem('playerGuid')
    if (guid == null) {
      // get new guid
      this.$axios
        .get('/api/Players/ValidatePlayerGuid?playerGuid=invalid')
        .then((response) => {
          this.playerGuid = response.data
        })
    } else {
      // Check player guid for validity
      this.$axios
        .get('/api/Players/ValidatePlayerGuid?playerGuid=' + guid)
        .then((response) => {
          this.playerGuid = response.data
        })
    }
  }

  setUser(userName: string) {
    localStorage.setItem('userName', userName)
    localStorage.setItem('playerGuid', this.playerGuid)
    if (this.wordleGame.state === GameState.Won) {
      this.endGameSave()
    }
  }

  startTimer() {
    this.startTime = Date.now() / 1000
    this.intervalID = setInterval(this.updateTimer, 1000)
  }

  updateTimer() {
    this.timeInSeconds = Math.floor(Date.now() / 1000 - this.startTime)
  }

  stopTimer() {
    this.endTime = Date.now() / 1000
    clearInterval(this.intervalID)
  }

  displayTimer() {
    let text = `${
      this.timeInSeconds / 60 / 60 > 1
        ? Math.floor(this.timeInSeconds / 60 / 60) + ':'
        : ''
    }`
    text += `${
      Math.floor((this.timeInSeconds / 60) % 60) < 10
        ? '0' + Math.floor((this.timeInSeconds / 60) % 60)
        : Math.floor((this.timeInSeconds / 60) % 60)
    }:`
    text += `${
      Math.floor(this.timeInSeconds % 60) < 10
        ? '0' + Math.floor(this.timeInSeconds % 60)
        : Math.floor(this.timeInSeconds % 60)
    }`
    return text
  }

  endGameSave() {
    this.$axios.post('/api/Players', {
      name: this.playerName,
      playerGuid: this.playerGuid,
      attempts: this.wordleGame.words.length,
      seconds: this.timeInSeconds,
    })
  }
}
</script>
