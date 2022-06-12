<template>
  <div class>
    <v-container @click="toggleDialog">
      Login <v-icon>mdi-account</v-icon>
    </v-container>

    <v-dialog v-model="dialog" width="450">
      <v-card>
        <v-container>
          <v-card-title>Login</v-card-title>

          <v-card-text>
            <v-text-field
              v-model="username"
              label="Username"
              required
            ></v-text-field>
            <v-text-field
              v-model="password"
              type="password"
              label="Password"
              required
            ></v-text-field>
            <v-btn @click="Login()">SUBMIT</v-btn>
          </v-card-text>
        </v-container>
      </v-card>
    </v-dialog>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from 'vue-property-decorator'
import { JWT } from '~/scripts/jwt'

@Component({})
export default class LoginDialog extends Vue {
  dialog = false
  username: string = ''
  password: string = ''

  toggleDialog() {
    this.dialog = !this.dialog
  }

  Login() {
    this.$axios
      .post('Token/GetToken', {
        username: this.username,
        password: this.password,
      })
      .then((result) => {
        JWT.setToken(result.data.token, this.$axios)
        this.$axios.get('Token/TestAdmin').then(() => {
          this.dialog = false
        })
      })
  }
}
</script>
