<template>
  <v-card justify-center class="p-6">
    <v-card>
      <p id="playerName" @click="toggleDialog">Welcome, {{ getName() }}.</p>
    </v-card>

    <v-dialog
      v-model="dialog"
      persistent
      transition="dialog-top-transition"
      max-width="256"
      class="p-6"
    >
      <v-card class="p-6" justify-center>
        <v-text-field
          v-model="playerName"
          min-width="128"
          label="Player Name"
          inverted
          class="float-center m-2"
        ></v-text-field>
        <v-btn
          min-width="128"
          justify-center
          class="m-4"
          @click="
            toggleDialog()
            setName()
          "
        >
          Submit
        </v-btn>
      </v-card>
    </v-dialog>
  </v-card>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
@Component({})
export default class PlayerInfo extends Vue {
  dialog = false
  playerName: string | null = this.getName()

  toggleDialog() {
    this.dialog = !this.dialog
  }

  getName(): string | null {
    if (localStorage.getItem('name')) {
      return localStorage.getItem('name')
    } else return 'Guest'
  }

  setName() {
    if (this.playerName != null) {
      localStorage.setItem('name', this.playerName)
    }
  }
}
</script>
