<template>
  <v-card>
    <v-row>
      <v-col class="text-center">
        <blockquote class="blockquote">
          <p>
            &#8220; This is a student project for Eastern Washington University.
            As a class we are developing Wordle with added components. &#8221;
          </p>
          <p>
            &#8220; Meg is awesome and is an amazing software developer! &#8221;
          </p>
          <footer>
            <small>
              <em>&mdash;Samuel Shaw &#38; Zachariah Ward</em>
            </small>
          </footer>
        </blockquote>
        <back-button />
      </v-col>
    </v-row>
  </v-card>
</template>

<script lang="ts">
export default {
  name: 'AboutPage',
}
</script>
