using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

//-------------------------------------------------------------------------------------
//
//  MusicPlayer
//  by Charlie Huguenard (www.charliehuge.com)
//
//  Scans your music library and plays songs from it, 
//  displaying song info on the screen.
//
//  To use, simpy make "musicPlayer" a member of your main game class, and add the 
//  following lines to the LoadContent function:
//      yourFont = Content.Load<SpriteFont>("YourFontName");
//      musicPlayer = new MusicPlayer(this, yourFont, spriteBatch);
//
//-------------------------------------------------------------------------------------
public class MusicPlayer : DrawableGameComponent
{
    MediaLibrary library;
    SongCollection songs;
    Song currentSong;

    Random random = new Random();

    SpriteFont font;
    SpriteBatch spriteBatch;

    public MusicPlayer(Game game, SpriteFont font, SpriteBatch spriteBatch) 
        : base(game)
    {
        game.Components.Add(this);

        library = new MediaLibrary();
        songs = library.Songs;
        this.font = font;
        this.spriteBatch = spriteBatch;
                        
        MediaPlayer.MediaStateChanged += new EventHandler<EventArgs>(MediaStateChangedHandler);

        RandomSong();
    }

    //----------------------------------------------------------
    //
    //  MediaStateChangedHandler
    //      Fired off when the MediaPlayer's state changes.
    //      Use this to switch songs when one finishes.
    //
    //----------------------------------------------------------
    void MediaStateChangedHandler(object sender, EventArgs e)
    {
        if (MediaPlayer.State == MediaState.Stopped)
            RandomSong();
    }

    //----------------------------------------------------------
    //
    //  NextSong
    //      Starts playing the next song in the library
    //
    //----------------------------------------------------------
    public void NextSong()
    {
        //TODO
    }

    //----------------------------------------------------------
    //
    //  RandomSong
    //      Picks a new song at random, starts playing
    //
    //----------------------------------------------------------
    public void RandomSong()
    {
        currentSong = songs[random.Next(songs.Count)];
        PlaySong();
    }

    //----------------------------------------------------------
    //
    //  PlaySong
    //
    //----------------------------------------------------------
    void PlaySong()
    {
        MediaPlayer.Play(currentSong);
    }

    //----------------------------------------------------------
    //
    //  ToggleMute
    //
    //----------------------------------------------------------
    void ToggleMute()
    {
        if (MediaPlayer.Volume > 0f)
            MediaPlayer.Volume = 0f;
        else
            MediaPlayer.Volume = 1f;
    }

    //----------------------------------------------------------
    //
    //  Draw
    //
    //----------------------------------------------------------
    public override void Draw(GameTime gameTime)
    {
        string songInfo = currentSong.Name + " | " + currentSong.Artist + " | " + FormatTimeSpan(MediaPlayer.PlayPosition) + "/" + FormatTimeSpan(currentSong.Duration);
        spriteBatch.Begin();
        spriteBatch.DrawString(font, songInfo, Vector2.Zero, Color.White);
        spriteBatch.End();
    }

    //----------------------------------------------------------
    //
    //  FormatTimeSpan
    //      Put timeSpan into a human-readable format
    //
    //----------------------------------------------------------
    string FormatTimeSpan(TimeSpan timeSpan)
    {
        int minutes = timeSpan.Minutes;
        int seconds = timeSpan.Seconds;

        if (seconds < 10)
            return minutes + ":0" + seconds;
        else
            return minutes + ":" + seconds;
    }
}