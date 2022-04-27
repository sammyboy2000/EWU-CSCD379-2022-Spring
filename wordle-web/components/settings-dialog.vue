<template>
  <div>
    <v-btn icon @click="toggleDialog">
      <v-icon> mdi-cog </v-icon>
    </v-btn>

    <v-dialog v-model="dialog" width="450">
      <v-card>
        <v-container>
          <router-link
            to="/about"
            class="text-h4 font-weight-black no-decoration float-right"
          >
            <v-icon @click="toggleDialog">mdi-information</v-icon>
          </router-link>
          <v-card-title> Settings </v-card-title>

          <v-card-text>
            <v-menu offset-y>
              <template #activator="{ on, attrs }">
                <v-btn color="primary" dark v-bind="attrs" v-on="on">
                  Pick Theme
                </v-btn>
              </template>
              <v-list>
                <v-list-item-group>
                  <v-list-item @click="turnOnTheLights">
                    <v-list-item-title> Light Mode </v-list-item-title>
                  </v-list-item>
                  <v-list-item @click="turnOffTheLights">
                    <v-list-item-title> Dark Mode </v-list-item-title>
                  </v-list-item>
                </v-list-item-group>
              </v-list>
            </v-menu>

            <v-menu offset-y>
              <template #activator="{ on, attrs }">
                <v-btn color="primary" dark v-bind="attrs" v-on="on">
                  Pick A Color
                </v-btn>
              </template>
              <v-list>
                <v-list-item-group>
                  <v-list-item @click="purpleTheme">
                    <v-list-item-title> Lavender </v-list-item-title>
                  </v-list-item>
                  <v-list-item @click="greenTheme">
                    <v-list-item-title> Forest </v-list-item-title>
                  </v-list-item>
                  <v-list-item @click="blueTheme">
                    <v-list-item-title> Blueberry </v-list-item-title>
                  </v-list-item>
                </v-list-item-group>
              </v-list>
            </v-menu>
          </v-card-text>
        </v-container>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { colors } from 'vuetify/lib'

@Component({})
export default class SettingsDialog extends Vue {
  dialog = false

  toggleDialog() {
    this.dialog = !this.dialog
  }

  turnOnTheLights() {
    this.$vuetify.theme.dark = false
  }

  turnOffTheLights() {
    this.$vuetify.theme.dark = true
  }

  purpleTheme() {
    const purpleTheme = {
      primary: colors.deepPurple.darken1,
      accent: colors.pink.accent3,
      secondary: colors.pink,
      info: '#03A9F4',
      warning: colors.pink.accent1,
      error: colors.blueGrey.lighten1,
      success: colors.deepPurple.darken2,
    }

    this.$vuetify.theme.themes.dark = purpleTheme
    this.$vuetify.theme.themes.light = purpleTheme
  }

  greenTheme() {
    const greenTheme = {
      primary: colors.green.darken4,
      accent: colors.lightGreen.accent3,
      secondary: colors.green.lighten5,
      info: '#03e0f4',
      warning: colors.green.lighten1,
      error: colors.lightGreen.lighten4,
      success: colors.green.darken4,
    }

    this.$vuetify.theme.themes.dark = greenTheme
    this.$vuetify.theme.themes.light = greenTheme
  }

  blueTheme() {
    const blueTheme = {
      primary: colors.blue.darken4,
      accent: colors.lightBlue.accent3,
      secondary: colors.blue.lighten5,
      info: '#009688',
      warning: colors.blue.lighten1,
      error: colors.blueGrey.lighten4,
      success: colors.blue.darken4,
    }

    this.$vuetify.theme.themes.dark = blueTheme
    this.$vuetify.theme.themes.light = blueTheme
  }
}
</script>
