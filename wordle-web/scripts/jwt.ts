import { NuxtAxiosInstance } from '@nuxtjs/axios'
class WordleToken {
  Birthday: string = ''
  Random: string = ''
  UserId: string = ''
  UserName: string = ''
  aud: string = ''
  exp: number = 0
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string[] = []
  iss: string = ''
  jti: string = ''
  sub: string = ''
  get roles(): string[] {
    return this['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
  }

  get age(): number {
    const bday: Date = new Date(Date.parse(this.Birthday))
    const tday: Date = new Date()
    const yearDiff = tday.getFullYear() - bday.getFullYear()
    const monthDiff = tday.getMonth() - bday.getMonth()
    const dateDiff = tday.getDate() - bday.getDate()
    if (monthDiff > 0 || (monthDiff === 0 && dateDiff >= 0)) {
      return yearDiff
    } else {
      return yearDiff - 1
    }
  }
}
export class JWT {
  private static tokenInstance: string
  private static _tokenData: WordleToken

  public static setToken(token: string | null, axios: NuxtAxiosInstance) {
    if (token != null) {
      this.tokenInstance = token
      localStorage.setItem('token', this.tokenInstance)
      axios.setHeader('Authorization', `Bearer ${token}`)
      const parts = token.split('.')
      const payload = JSON.parse(atob(parts[1]))
      this._tokenData = Object.assign(new WordleToken(), payload)
    }
  }

  public static getToken(): string {
    return this.tokenInstance
  }

  public static get tokenData(): WordleToken {
    return this._tokenData
  }
}
