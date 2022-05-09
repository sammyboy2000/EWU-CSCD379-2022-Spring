import { WordsService } from '@/scripts/wordsService'
import { Word } from '~/scripts/word'

describe('Word Service', () => {
  test('Get a word', () => {
    const word = WordsService.getRandomWord()
    expect(word).not.toBeNull()
    expect(word).toHaveLength(5)
    expect(word).not.toHaveLength(4)
  })

  test('Words are private', () => {
    expect((WordsService as any).words).toBeUndefined()
  })

  describe('find valid words', () => {
    test('find a valid words', () => {
      const testWord = new Word()
      testWord.addLetter('a')
      testWord.addLetter('d')
      testWord.addLetter('e')
      testWord.addLetter('p')
      testWord.addLetter('t')
      const words = WordsService.validWords(testWord)
      expect(words).not.toBeNull()
      expect(words).toHaveLength(1)
    })

    test('find valid words starting with u', () => {
      const testWord = new Word()
      testWord.addLetter('u')
      testWord.addLetter('?')
      testWord.addLetter('?')
      testWord.addLetter('?')
      testWord.addLetter('?')
      const words = WordsService.validWords(testWord)
      expect(words).not.toBeNull()
      expect(words).toHaveLength(7)
    })

    test('find valid words starting with u but short', () => {
      const testWord = new Word()
      testWord.addLetter('u')
      const words = WordsService.validWords(testWord)
      expect(words).not.toBeNull()
      expect(words).toHaveLength(7)
    })

    test('find valid words starting with umb', () => {
      const testWord = new Word()
      testWord.addLetter('u')
      testWord.addLetter('m')
      testWord.addLetter('b')
      testWord.addLetter('?')
      testWord.addLetter('?')
      const words = WordsService.validWords(testWord)
      expect(words).not.toBeNull()
      expect(words).toHaveLength(2)
    })

    test('find valid words complex', () => {
      const testWord = new Word()
      testWord.addLetter('u')
      testWord.addLetter('?')
      testWord.addLetter('b')
      testWord.addLetter('?')
      testWord.addLetter('?')
      const words = WordsService.validWords(testWord)
      expect(words).not.toBeNull()
      expect(words).toHaveLength(3)
    })

    test('find valid words ending with x', () => {
      const testWord = new Word()
      testWord.addLetter('?')
      testWord.addLetter('?')
      testWord.addLetter('?')
      testWord.addLetter('?')
      testWord.addLetter('x')
      const words = WordsService.validWords(testWord)
      expect(words).not.toBeNull()
      expect(words).toHaveLength(4)
    })
  })
})
describe('Search Results Test', () => {
  test('Finds correct results', () => {
    let candWords = WordsService.validWords('z')
    expect(candWords.length === 1 && candWords[0] === 'zebra')
    candWords = WordsService.validWords('zeb')
    expect(candWords.length === 1 && candWords[0] === 'zebra')
    candWords = WordsService.validWords('??z')
    expect(candWords.length === 4 && candWords.includes('hazel'))
    candWords = WordsService.validWords('?z?z')
    expect(candWords.length === 0)
    candWords = WordsService.validWords('qv')
    expect(candWords.length === 0)
    candWords = WordsService.validWords('')
    expect(candWords.length === 631)
  })
})
