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
    const testWord = new Word()
    testWord.addLetter('z')
    let candWords = WordsService.validWords(testWord)
    expect(candWords.length === 1 && candWords[0] === 'zebra')
    testWord.addLetter('e')
    testWord.addLetter('b')
    candWords = WordsService.validWords(testWord)
    expect(candWords.length === 1 && candWords[0] === 'zebra')

    testWord.removeLetter()
    testWord.removeLetter()
    testWord.removeLetter()

    testWord.addLetter('?')
    testWord.addLetter('?')
    testWord.addLetter('z')

    candWords = WordsService.validWords(testWord)
    expect(candWords.length === 4 && candWords.includes('hazel'))

    testWord.removeLetter()
    testWord.removeLetter()
    testWord.addLetter('z')
    testWord.addLetter('?')
    testWord.addLetter('z')
    candWords = WordsService.validWords(testWord)
    expect(candWords.length === 0)
    testWord.removeLetter()
    testWord.removeLetter()
    testWord.removeLetter()
    testWord.removeLetter()
    testWord.addLetter('q')
    testWord.addLetter('v')
    candWords = WordsService.validWords(testWord)
    expect(candWords.length === 0)
    testWord.removeLetter()
    testWord.removeLetter()
    candWords = WordsService.validWords(testWord)
    expect(candWords.length === 631)
  })
})
