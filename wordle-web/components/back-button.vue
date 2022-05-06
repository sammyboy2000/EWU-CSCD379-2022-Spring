<template>
  <span class="back-button">
    <v-btn icon @click="back"
      ><v-icon x-large color="secondary">mdi-arrow-left-circle</v-icon></v-btn
    >
  </span>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component
export default class BackButton extends Vue {
  back() {
    return this.$router.go(-1)
  }
}
</script>
