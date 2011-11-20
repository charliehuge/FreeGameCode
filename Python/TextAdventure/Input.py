#-------------------------------------------------------------
#
#   Input.py
#
#   (c) 2011 Charlie Huguenard
#
#-------------------------------------------------------------
__author__ = 'charlie'

import UI
import Game

def ParseText(text):
    words = text.lower().split()
    # see if the player specified a hotspot name
    for hs in Game.currentRoom.hotspots:
        for word in words:
            if word == hs.name.lower():
                # see if the player specified a verb the hotspot has
                for verb, response in hs.verbs.iteritems():
                    for word2 in words:
                        if word2 == verb.lower():
                            hs.Activate(verb)
                            return

    UI.AddText('I don\'t think that word means what you think it means.\n\n')