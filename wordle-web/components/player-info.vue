<template>
  <v-card justify-center class="p-6">
    <v-card>
      <v-btn id="playerName" max-width="128" @click="toggleDialog">{{ getName() }}</v-btn>
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
        ></v-text-field>
        <v-btn
          min-width="128"
          max-width="128"
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

  public getName(): string | null {
    if (localStorage.getItem('name')) {
      return localStorage.getItem('name')
    } else return 'Guest'
  }

  setName(name: string) {
    if(name != null){
      localStorage.setItem('name', name)
      return
    }
    if(this.playerName != null) {
      localStorage.setItem('name', this.playerName)
    }
  }
}
</script>
