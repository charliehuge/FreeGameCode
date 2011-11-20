#-------------------------------------------------------------
#
#   Game.py
#
#   (c) 2011 Charlie Huguenard
#
#-------------------------------------------------------------
__author__ = 'charlie'

from Room import Room

currentRoom = 0

def Init(startRoom):
    global currentRoom
    currentRoom = Room(startRoom)