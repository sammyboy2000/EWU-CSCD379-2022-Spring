<template>
  <v-container fluid fill-height>
    <v-container v-if="!isLoaded">
      <v-row justify="center">
        <v-card loading>
          <v-card-title class="justify-center">
            Game is Loading...
          </v-card-title>
          <PrerollAd />
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
                  @click=";(dialog = false), setUserName(playerName)"
                >
                  Save
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-col>
      </v-row>
      <v-row v-if="!isSmall()">
        <v-col cols="1"></v-col>
        <v-col cols="10" class="mt-0 mb-0 pt-0 pb-0">
          <v-img
            src="logo.jpeg"
            class="center"
            style="max-width: 100%; height: auto"
          />
        </v-col>
        <v-col cols="1"> </v-col>
      </v-row>
      <v-row justify="center" class="mt-10">
        <v-alert v-if="wordleGame.gameOver" width="80%" :type="gameResult.type">
          {{ gameResult.text }}
          <v-btn class="ml-2" @click="resetGame"> Play Again?</v-btn>
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
import { WordsService } from '~/scripts/wordsService'
import { GameState, WordleGame } from '~/scripts/wordleGame'
import KeyBoard from '@/components/keyboard.vue'
import GameBoard from '@/components/game-board.vue'
import { Word } from '~/scripts/word'
import { Stopwatch } from '~/scripts/stopwatch'
import {JWT} from '~/scripts/jwt'

@Component({ components: { KeyBoard, GameBoard } })
export default class Game extends Vue {
  stopwatch: Stopwatch = new Stopwatch()
  // ? need this for closing button
  dialog: boolean = false
  playerName: string = ''
  playerGuid: string = ''
  timeInSeconds: number = 0
  startTime: number = 0
  endTime: number = 0
  intervalID: any
  word: string = WordsService.getRandomWord()
  wordleGame = new WordleGame(this.word)
  isLoaded: boolean = false

  isSmall() {
    return this.$vuetify.breakpoint.smAndDown
  }

  mounted() {
    if (!this.stopwatch.isRunning) {
      this.stopwatch.Start()
    }
    this.retrieveGuid()
    this.retrieveUserName()
    this.$axios
      .post('Token/GetToken', {
        username: 'Admin@intellitect.com',
        password: 'P@ssw0rd123',
      })
      .then((result) => {
          JWT.setToken(result.data.token,this.$axios)
        // console.log(result)
         console.log(JWT.tokenData)
        console.log(JWT.tokenData.roles)
        // this.$axios.defaults.headers.common.Authorization =
        //   'Bearer ' + result.data.token
        this.$axios.get('Token/TestAdmin').then((result) => {
          //console.log(result)
        })
      })
  }

  displayTimer(): string {
    return this.stopwatch.getFormattedTime()
  }

  resetGame() {
    this.word = WordsService.getRandomWord()
    this.wordleGame = new WordleGame(this.word)
    this.stopwatch.Start()
  }

  get gameResult() {
    this.stopwatch.Stop()
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
    let guid = localStorage.getItem('playerGuid')
    if (guid == null) {
      this.$axios
        .get('/api/Players/ValidatePlayerGuid?playerGuid=invalid')
        .then((response) => {
          this.playerGuid = response.data
        })
    } else {
      this.$axios
        .get('/api/Players/ValidatePlayerGuid?playerGuid=' + guid)
        .then((response) => {
          this.playerGuid = response.data
        })
    }
  }

  setUserName(userName: string) {
    localStorage.setItem('userName', userName)
    localStorage.setItem('playerGuid', this.playerGuid)
    if (this.wordleGame.state === GameState.Won) {
      this.endGameSave()
    }
  }

  endGameSave() {
    this.$axios.post('/api/Players', {
      name: this.playerName,
      playerGuid: this.playerGuid,
      attempts: this.wordleGame.words.length,
      seconds: Math.round(this.stopwatch.currentTime / 1000),
    })
  }
}
</script>
