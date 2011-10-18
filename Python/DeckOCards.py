#-------------------------------------------------------------------
#
#	Deck o' Cards
#	A quick exercise I did to refresh my memory for Python.
#
#-------------------------------------------------------------------
__author__ = 'charlie'

import random

class Card:
    def __init__(self, suit, value):
        self.suit = suit
        self.value = value

    def Print(self):
        print self.value + self.suit,

class Deck:
    values = ('2','3','4','5','6','7','8','9','J','Q','K','A')
    suits = ('H','D','C','S')

    def __init__(self):
        self.nextCard = 0
        self.cards = []
        for suit in self.suits:
            for value in self.values:
                self.cards.append(Card(suit, value))

    def Shuffle(self):
        random.shuffle(self.cards)

    def DealCard(self):
        curCard = self.nextCard
        self.nextCard += 1
        return self.cards[curCard]

class Hand:
    def __init__(self):
        self.cards = []

    def AddCard(self, card):
        self.cards.append(card)

    def PrintHand(self):
        for card in self.cards:
            card.Print(),


deck = Deck()
deck.Shuffle()

hand1 = Hand()
hand2 = Hand()

for i in range(0,5):
    hand1.AddCard(deck.DealCard())
    hand2.AddCard(deck.DealCard())

hand1.PrintHand()
print ''
print ''
hand2.PrintHand()