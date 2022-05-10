<template>
  <v-app>
    <v-app-bar fixed app>
      <router-link 
      to="/"
      class="text-h4 font-weight-black no-decoration ma-0 pa-0">
        !<v-icon class="notwordle-logo" size="64">mdi-file-word-box</v-icon>ordle
        </router-link>
      <v-spacer />
      <settings-dialog />
      <hamburger-menu />
    </v-app-bar>
    <v-main>
      <v-container>
        <Nuxt />
      </v-container>
    </v-main>
    <v-footer app>
      <span>&copy; {{ new Date().getFullYear() }}</span>
    </v-footer>
  </v-app>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import SettingsDialog from '@/components/settings-dialog.vue'

@Component({ components: { SettingsDialog } })
export default class DefaultLayout extends Vue {}
</script>

<style>
.no-decoration {
  text-decoration: none;
}

.notwordle-logo {
  max-height: 180px;
  max-width: 180px;
  transform: rotateY(720deg);
  animation: turn 2.5s ease-out forwards 0.5s;
  animation-iteration-count: infinite;
}

.notwordle-logo:hover {
  animation-name: turn;
  animation-timing-function: ease-in-out;
}

@keyframes turn {
  100% {
    transform: rotateY(0deg);
  }
}
</style>
