#-------------------------------------------------------------
#
#   Room.py
#
#   (c) 2011 Charlie Huguenard
#
#-------------------------------------------------------------
__author__ = 'charlie'

from xml.dom import minidom
import UI

class HotSpot:
    def __init__(self, name, startsVisible):
        self.name = name
        self.visible = startsVisible
        self.verbs = {}

    def AddVerb(self, verb, response):
        self.verbs[verb] = response

    def Activate(self, verb):
        response = self.verbs[verb]
        if self.visible and response:
            response.Activate()
        else:
            print("No verb by that name:", verb)

class Response:
    def __init__(self, text, function):
        self.function = function
        self.text = text

    def Activate(self):
        if self.function is not None:
            pass # TODO: callback
        if self.text is not None:
            UI.AddText(self.text + "\n\n")

class Room:
    def __init__(self, file):
        self.hotspots = []
        self.Load(file)

    def AddHotSpot(self, hotspot):
        self.hotspots.append(hotspot)

    def OnEnter(self):
        # fire the room description (will always be called "room")
        UI.AddText(self.name + ": ")
        for hotspot in self.hotspots:
            if hotspot.name.lower() == "room":
                hotspot.Activate("look")

    def Test(self):
        print '\nRunning room test for',self.name
        for hotspot in self.hotspots:
            for verb, response in hotspot.verbs.iteritems():
                print verb, hotspot.name
                hotspot.Activate(verb)
        print ''

    def Load(self, file):
        roomData = minidom.parse(file).firstChild
        self.name = roomData.attributes["name"].value
        hotspotList = roomData.getElementsByTagName("Hotspot")
        # Add Hotspots
        for hotspot in hotspotList:
            hsName = hotspot.attributes["name"].value
            visible = hotspot.attributes["startsVisible"].value
            visible = visible.lower() == "true"
            hs = HotSpot(hsName, visible)
            self.AddHotSpot(hs)
            # Add Verbs
            verbList = hotspot.getElementsByTagName("Verb")
            for verb in verbList:
                # TODO: rotating text, callbacks, etc
                # for now, just grab the first text
                verbName = verb.attributes["name"].value
                responseText = verb.getElementsByTagName("Text")[0].firstChild.data
                response = Response(responseText, None)
                hs.AddVerb(verbName, response)


