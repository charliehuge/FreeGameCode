#-------------------------------------------------------------
#
#   UI.py
#
#   (c) 2011 Charlie Huguenard
#
#-------------------------------------------------------------
__author__ = 'charlie'

import web
import Game
import Input

currentText = ""

class UI:
    urls = ('/', 'UI')
    app = web.application(urls, globals())
    render = web.template.render('templates/')

    def GET(self):
        ClearText()
        Game.Init("RoomData/TestRoom.xml")
        Game.currentRoom.OnEnter()
        InputPrompt()
        return self.render.UI(currentText)
    
    def POST(self):
        inString = self.ParsePostData(web.data())["curString"]
        AddText(inString + "\n\n")
        Input.ParseText(inString)
        InputPrompt()
        return currentText

    def Run(self):
        self.app.run()

    def ParsePostData(self, raw):
        rawlist = raw.split('&')
        parsedList = {}
        for item in rawlist:
            tmpList = item.split("=")
            parsedList[tmpList[0]] = tmpList[1].replace("+"," ")
        return parsedList
            


def InputPrompt():
    AddText("\nWhat would you like to do?\n> ")

def AddText(text):
    formattedText = text.replace("\n","<br />")
    global currentText
    currentText += formattedText

def ClearText():
    global currentText
    currentText = ""