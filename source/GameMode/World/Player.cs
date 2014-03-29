﻿// SampSharp
// Copyright (C) 2014 Tim Potze
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>

using System;
using System.Collections.Generic;
using System.Linq;
using GameMode.Definitions;
using GameMode.Events;

namespace GameMode.World
{
    /// <summary>
    ///     Represents a SA:MP player.
    /// </summary>
    public class Player : IWorldObject, IDisposable
    {
        #region Fields

        /// <summary>
        ///     Gets an ID commonly returned by methods to point out that no player matched the requirements.
        /// </summary>
        public const int InvalidId = Misc.InvalidPlayerId;

        /// <summary>
        ///     Contains all instances of Players.
        /// </summary>
        protected static List<Player> Instances = new List<Player>();

        #endregion

        #region Factories

        /// <summary>
        ///     Returns an instance of <see cref="Player" /> that deals with <paramref name="playerId" />.
        /// </summary>
        /// <param name="playerId">The ID of the player we are dealing with.</param>
        /// <returns>An instance of <see cref="Player" />.</returns>
        public static Player Find(int playerId)
        {
            //Find player in memory or initialize new player
            return Instances.FirstOrDefault(p => p.PlayerId == playerId) ?? new Player(playerId);
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initalizes a new instance of the Player class.
        /// </summary>
        /// <param name="playerId">The ID of the player to initialize.</param>
        protected Player(int playerId)
        {
            //Fill properties
            PlayerId = playerId;

            Instances.Add(this);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the ID of this Player.
        /// </summary>
        public int PlayerId { get; private set; }

        /// <summary>
        ///     Gets a readonly set of all <see cref="Player" /> instances.
        /// </summary>
        public static IReadOnlyCollection<Player> All
        {
            get { return Instances.AsReadOnly(); }
        }

        #endregion

        #region Players properties

        /// <summary>
        ///     Gets or sets the name of this Player.
        /// </summary>
        public virtual string Name
        {
            get { return Native.GetPlayerName(PlayerId); }
            set { Native.SetPlayerName(PlayerId, value); }
        }

        /// <summary>
        ///     Gets or sets the position of this Player.
        /// </summary>
        public virtual Vector Position
        {
            get { return Native.GetPlayerPos(PlayerId); }
            set { Native.SetPlayerPos(PlayerId, value); }
        }


        /// <summary>
        ///     Gets or sets the rotation of this Player.
        /// </summary>
        /// <remarks>
        ///     Only the Z angle can be set!
        /// </remarks>
        public virtual Vector Rotation
        {
            get { return new Vector(0, 0, Angle); }
            set { Angle = value.Z; }
        }

        /// <summary>
        ///     Gets or sets the facing angle of this Player.
        /// </summary>
        public virtual float Angle
        {
            get { return Native.GetPlayerFacingAngle(PlayerId); }
            set { Native.SetPlayerFacingAngle(PlayerId, value); }
        }

        /// <summary>
        ///     Gets or sets the interior of this Player.
        /// </summary>
        public virtual int Interior
        {
            get { return Native.GetPlayerInterior(PlayerId); }
            set { Native.SetPlayerInterior(PlayerId, value); }
        }

        /// <summary>
        ///     Gets or sets the virtual world of this Player.
        /// </summary>
        public virtual int VirtualWorld
        {
            get { return Native.GetPlayerVirtualWorld(PlayerId); }
            set { Native.SetPlayerVirtualWorld(PlayerId, value); }
        }

        /// <summary>
        ///     Gets or sets the health of this Player.
        /// </summary>
        public virtual float Heath
        {
            get { return Native.GetPlayerHealth(PlayerId); }
            set { Native.SetPlayerHealth(PlayerId, value); }
        }

        /// <summary>
        ///     Gets or sets the armour of this Player.
        /// </summary>
        public virtual float Armour
        {
            get { return Native.GetPlayerArmour(PlayerId); }
            set { Native.SetPlayerArmour(PlayerId, value); }
        }

        /// <summary>
        ///     Gets the ammo of the Weapon this Player is currently holding.
        /// </summary>
        public virtual int WeaponAmmo
        {
            get { return Native.GetPlayerAmmo(PlayerId); }
        }

        /// <summary>
        ///     Gets the WeaponState of the Weapon this Player is currently holding.
        /// </summary>
        public virtual WeaponState WeaponState
        {
            get { return (WeaponState) Native.GetPlayerWeaponState(PlayerId); }
        }

        /// <summary>
        ///     Gets the Weapon this Player is currently holding.
        /// </summary>
        public virtual Weapon Weapon
        {
            get { return (Weapon) Native.GetPlayerWeapon(PlayerId); }
        }

        /// <summary>
        ///     Gets the Player this Player is aiming at.
        /// </summary>
        public virtual Player TargetPlayer
        {
            get
            {
                int target = Native.GetPlayerTargetPlayer(PlayerId);
                return target == InvalidId ? null : Find(target);
            }
        }

        /// <summary>
        ///     Gets or sets the team this Player is in.
        /// </summary>
        public virtual int Team
        {
            get { return Native.GetPlayerTeam(PlayerId); }
            set { Native.SetPlayerTeam(PlayerId, value); }
        }

        /// <summary>
        ///     Gets or sets the score of this Player.
        /// </summary>
        public virtual int Score
        {
            get { return Native.GetPlayerScore(PlayerId); }
            set { Native.SetPlayerScore(PlayerId, value); }
        }

        /// <summary>
        ///     Gets or sets the drunkness level of this Player.
        /// </summary>
        public virtual int DrunkLevel
        {
            get { return Native.GetPlayerDrunkLevel(PlayerId); }
            set { Native.SetPlayerDrunkLevel(PlayerId, value); }
        }

        /// <summary>
        ///     Gets or sets the Color of this Player.
        /// </summary>
        public virtual Color Color
        {
            get { return new Color(Native.GetPlayerColor(PlayerId)); }
            set { Native.SetPlayerColor(PlayerId, value); }
        }

        /// <summary>
        ///     Gets or sets the skin of this Player.
        /// </summary>
        public virtual int Skin
        {
            get { return Native.GetPlayerSkin(PlayerId); }
            set { Native.SetPlayerSkin(PlayerId, value); }
        }

        /// <summary>
        ///     Gets or sets the money of this Player.
        /// </summary>
        public virtual int Money
        {
            get { return Native.GetPlayerMoney(PlayerId); }
            set { Native.GivePlayerMoney(PlayerId, Money + value); }
        }

        /// <summary>
        ///     Gets the state of this Player.
        /// </summary>
        public virtual PlayerState PlayerState
        {
            get { return (PlayerState) Native.GetPlayerState(PlayerId); }
        }

        /// <summary>
        ///     Gets the IP of this Player.
        /// </summary>
        public virtual string IP
        {
            get { return Native.GetPlayerIp(PlayerId); }
        }

        /// <summary>
        ///     Gets the ping of this Player.
        /// </summary>
        public virtual int Ping
        {
            get { return Native.GetPlayerPing(PlayerId); }
        }

        /// <summary>
        ///     Gets or sets the wanted level of this Player.
        /// </summary>
        public virtual int WantedLevel
        {
            get { return Native.GetPlayerWantedLevel(PlayerId); }
            set { Native.SetPlayerWantedLevel(PlayerId, value); }
        }

        /// <summary>
        ///     Gets or sets the FightStyle of this Player.
        /// </summary>
        public virtual FightStyle FightStyle
        {
            get { return (FightStyle) Native.GetPlayerFightingStyle(PlayerId); }
            set { Native.SetPlayerFightingStyle(PlayerId, (int) value); }
        }

        /// <summary>
        ///     Gets or sets the velocity of this Player.
        /// </summary>
        public virtual Vector Velocity
        {
            get
            {
                float x, y, z;
                Native.GetPlayerVelocity(PlayerId, out x, out y, out z);
                return new Vector(x, y, z);
            }
            set { Native.SetPlayerVelocity(PlayerId, value.X, value.Y, value.Z); }
        }

        /// <summary>
        ///     Gets the vehicle seat this Player sits on.
        /// </summary>
        public virtual int VehicleSeat
        {
            get { return Native.GetPlayerVehicleSeat(PlayerId); }
        }

        /// <summary>
        ///     Gets the index of the animation this Player is playing.
        /// </summary>
        public virtual int AnimationIndex
        {
            get { return Native.GetPlayerAnimationIndex(PlayerId); }
        }

        /// <summary>
        ///     Gets or sets the SpecialAction of this Player.
        /// </summary>
        public virtual SpecialAction SpecialAction
        {
            get { return (SpecialAction) Native.GetPlayerSpecialAction(PlayerId); }
            set { Native.SetPlayerSpecialAction(PlayerId, value); }
        }

        /// <summary>
        ///     Gets the position of this Players's camera.
        /// </summary>
        public virtual Vector CameraPosition
        {
            get { return Native.GetPlayerCameraPos(PlayerId); }
        }

        /// <summary>
        ///     Gets the front vector of this Player's camera.
        /// </summary>
        public virtual Vector CameraFrontVector
        {
            get { return Native.GetPlayerCameraFrontVector(PlayerId); }
        }

        /// <summary>
        ///     Gets the mode of this Player's camera.
        /// </summary>
        public virtual CameraMode CameraMode
        {
            get { return (CameraMode) Native.GetPlayerCameraMode(PlayerId); }
        }

        /// <summary>
        ///     Gets whether this Player is currently in any vehicle.
        /// </summary>
        public virtual bool InAnyVehicle
        {
            get { return Native.IsPlayerInAnyVehicle(PlayerId); }
        }

        /// <summary>
        ///     Gets whether this Player is in his checkpoint.
        /// </summary>
        public virtual bool InCheckpoint
        {
            get { return Native.IsPlayerInCheckpoint(PlayerId); }
        }

        /// <summary>
        ///     Gets whether this Player is in his race-checkpoint.
        /// </summary>
        public virtual bool InRaceCheckpoint
        {
            get { return Native.IsPlayerInRaceCheckpoint(PlayerId); }
        }

        /// <summary>
        ///     Gets the Vehicle that this Player is surfing.
        /// </summary>
        public virtual Vehicle SurfingVehicle
        {
            get
            {
                int vehicleid = Native.GetPlayerSurfingVehicleID(PlayerId);
                return vehicleid == Vehicle.InvalidId ? null : Vehicle.Find(vehicleid);
            }
        }

        /// <summary>
        ///     Gets the Vehicle this Player is currently in.
        /// </summary>
        public virtual Vehicle Vehicle
        {
            get
            {
                int vehicleid = Native.GetPlayerVehicleID(PlayerId); //Returns 0, not Vehicle.InvalidId!
                return vehicleid == 0 ? null : Vehicle.Find(vehicleid);
            }
        }

        /// <summary>
        ///     Gets whether this Player is connected to the server.
        /// </summary>
        public virtual bool IsConnected
        {
            get { return Native.IsPlayerConnected(PlayerId); }
        }

        #endregion

        #region SAMP properties

        /// <summary>
        ///     Gets whether this Player is an actual player or an NPC.
        /// </summary>
        public virtual bool IsNPC
        {
            get { return Native.IsPlayerNPC(PlayerId); }
        }

        /// <summary>
        ///     Gets whether this Player is logged into RCON.
        /// </summary>
        public virtual bool IsAdmin
        {
            get { return Native.IsPlayerAdmin(PlayerId); }
        }

        /// <summary>
        ///     Gets this Player's network stats and saves them into a string.
        /// </summary>
        public virtual string NetworkStats
        {
            get { return Native.GetPlayerNetworkStats(PlayerId); }
        }

        /// <summary>
        ///     Gets this Player's game version.
        /// </summary>
        public virtual string Version
        {
            get { return Native.GetPlayerVersion(PlayerId); }
        }

        /// <summary>
        ///     Gets this Player's GPCI string.
        /// </summary>
        public virtual string GPCI
        {
            get { return Native.gpci(PlayerId); }
        }

        /// <summary>
        ///     Gets the maximum number of players that can join the server, as set by the server var 'maxplayers' in server.cfg.
        /// </summary>
        public static int MaxPlayers
        {
            get { return Native.GetMaxPlayers(); }
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerConnect" /> is being called.
        ///     This callback is called when a player connects to the server.
        /// </summary>
        public event PlayerHandler Connected;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerDisconnect" /> is being called.
        ///     This callback is called when a player disconnects from the server.
        /// </summary>
        public event PlayerDisconnectedHandler Disconnected;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerSpawn" /> is being called.
        ///     This callback is called when a player spawns.
        /// </summary>
        public event PlayerHandler Spawned;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnGameModeInit" /> is being called.
        ///     This callback is triggered when the gamemode starts.
        /// </summary>
        public event PlayerDeathHandler Died;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerText" /> is being called.
        ///     Called when a player sends a chat message.
        /// </summary>
        public event PlayerTextHandler Text;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerCommandText" /> is being called.
        ///     This callback is called when a player enters a command into the client chat window, e.g. /help.
        /// </summary>
        public event PlayerTextHandler CommandText;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerRequestClass" /> is being called.
        ///     Called when a player changes class at class selection (and when class selection first appears).
        /// </summary>
        public event PlayerRequestClassHandler RequestClass;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerEnterVehicle" /> is being called.
        ///     This callback is called when a player starts to enter a vehicle, meaning the player is not in vehicle yet at the
        ///     time this callback is called.
        /// </summary>
        public event PlayerEnterVehicleHandler EnterVehicle;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerExitVehicle" /> is being called.
        ///     This callback is called when a player exits a vehicle.
        /// </summary>
        /// <remarks>
        ///     Not called if the player falls off a bike or is removed from a vehicle by other means such as using
        ///     <see cref="Native.SetPlayerPos(int,Vector)" />.
        /// </remarks>
        public event PlayerVehicleHandler ExitVehicle;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerStateChange" /> is being called.
        ///     This callback is called when a player exits a vehicle.
        /// </summary>
        /// <remarks>
        ///     Not called if the player falls off a bike or is removed from a vehicle by other means such as using
        ///     <see cref="Native.SetPlayerPos(int,Vector)" />.
        /// </remarks>
        public event PlayerStateHandler StateChanged;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerEnterCheckpoint" /> is being called.
        ///     This callback is called when a player enters the checkpoint set for that player.
        /// </summary>
        public event PlayerHandler EnterCheckpoint;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerLeaveCheckpoint" /> is being called.
        ///     This callback is called when a player leaves the checkpoint set for that player.
        /// </summary>
        public event PlayerHandler LeaveCheckpoint;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerEnterRaceCheckpoint" /> is being called.
        ///     This callback is called when a player enters a race checkpoint.
        /// </summary>
        public event PlayerHandler EnterRaceCheckpoint;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerLeaveRaceCheckpoint" /> is being called.
        ///     This callback is called when a player leaves the race checkpoint.
        /// </summary>
        public event PlayerHandler LeaveRaceCheckpoint;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerRequestSpawn" /> is being called.
        ///     Called when a player attempts to spawn via class selection.
        /// </summary>
        public event PlayerHandler RequestSpawn;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerPickUpPickup" /> is being called.
        ///     Called when a player picks up a pickup created with <see cref="Native.CreatePickup" />.
        /// </summary>
        public event PlayerPickupHandler PickUpPickup;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnEnterExitModShop" /> is being called.
        ///     This callback is called when a player enters or exits a mod shop.
        /// </summary>
        public event PlayerEnterModShopHandler EnterExitModShop;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerSelectedMenuRow" /> is being called.
        ///     This callback is called when a player selects an item from a menu.
        /// </summary>
        public event PlayerSelectedMenuRowHandler SelectedMenuRow;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerExitedMenu" /> is being called.
        ///     Called when a player exits a menu.
        /// </summary>
        public event PlayerHandler ExitedMenu;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerInteriorChange" /> is being called.
        ///     Called when a player changes interior.
        /// </summary>
        /// <remarks>
        ///     This is also called when <see cref="Native.SetPlayerInterior" /> is used.
        /// </remarks>
        public event PlayerInteriorChangedHandler InteriorChanged;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerKeyStateChange" /> is being called.
        ///     This callback is called when the state of any supported key is changed (pressed/released). Directional keys do not
        ///     trigger this callback.
        /// </summary>
        public event PlayerKeyStateChangedHandler KeyStateChanged;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerUpdate" /> is being called.
        ///     This callback is called everytime a client/player updates the server with their status.
        /// </summary>
        /// <remarks>
        ///     This callback is called very frequently per second per player, only use it when you know what it's meant for.
        /// </remarks>
        public event PlayerHandler Update;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerStreamIn" /> is being called.
        ///     This callback is called when a player is streamed by some other player's client.
        /// </summary>
        public event StreamPlayerHandler StreamIn;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerStreamOut" /> is being called.
        ///     This callback is called when a player is streamed out from some other player's client.
        /// </summary>
        public event StreamPlayerHandler StreamOut;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnDialogResponse" /> is being called.
        ///     This callback is called when a player responds to a dialog shown using <see cref="Native.ShowPlayerDialog" /> by
        ///     either clicking a button, pressing ENTER/ESC or double-clicking a list item (if using a list style dialog).
        /// </summary>
        public event DialogResponseHandler DialogResponse;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerTakeDamage" /> is being called.
        ///     This callback is called when a player takes damage.
        /// </summary>
        public event PlayerDamageHandler TakeDamage;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerGiveDamage" /> is being called.
        ///     This callback is called when a player gives damage to another player.
        /// </summary>
        /// <remarks>
        ///     One thing you can do with GiveDamage is detect when other players report that they have damaged a certain player,
        ///     and that player hasn't taken any health loss. You can flag those players as suspicious.
        ///     You can also set all players to the same team (so they don't take damage from other players) and process all health
        ///     loss from other players manually.
        ///     You might have a server where players get a wanted level if they attack Cop players (or some specific class). In
        ///     that case you might trust GiveDamage over TakeDamage.
        ///     There should be a lot you can do with it. You just have to keep in mind the levels of trust between clients. In
        ///     most cases it's better to trust the client who is being damaged to report their health/armour (TakeDamage). SA-MP
        ///     normally does this. GiveDamage provides some extra information which may be useful when you require a different
        ///     level of trust.
        /// </remarks>
        public event PlayerDamageHandler GiveDamage;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerClickMap" /> is being called.
        ///     This callback is called when a player places a target/waypoint on the pause menu map (by right-clicking).
        /// </summary>
        /// <remarks>
        ///     The Z value provided is only an estimate; you may find it useful to use a plugin like the MapAndreas plugin to get
        ///     a more accurate Z coordinate (or for teleportation; use <see cref="Native.SetPlayerPosFindZ(int,Vector)" />).
        /// </remarks>
        public event PlayerClickMapHandler ClickMap;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerClickTextDraw" /> is being called.
        ///     This callback is called when a player clicks on a textdraw or cancels the select mode(ESC).
        /// </summary>
        /// <remarks>
        ///     The clickable area is defined by <see cref="Native.TextDrawTextSize" />. The x and y parameters passed to that
        ///     function must not be zero or negative.
        /// </remarks>
        public event PlayerClickTextDrawHandler ClickTextDraw;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerClickPlayerTextDraw" /> is being called.
        ///     This callback is called when a player clicks on a player-textdraw. It is not called when player cancels the select
        ///     mode (ESC) - however, <see cref="BaseMode.OnPlayerClickTextDraw" /> is.
        /// </summary>
        public event PlayerClickTextDrawHandler ClickPlayerTextDraw;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerClickPlayer" /> is being called.
        ///     Called when a player double-clicks on a player on the scoreboard.
        /// </summary>
        /// <remarks>
        ///     There is currently only one 'source' (<see cref="PlayerClickSource.Scoreboard" />). The existence of this argument
        ///     suggests that more sources may be supported in the future.
        /// </remarks>
        public event PlayerClickPlayerHandler ClickPlayer;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerEditObject" /> is being called.
        ///     This callback is called when a player ends object edition mode.
        /// </summary>
        public event PlayerEditObjectHandler EditObject;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerEditAttachedObject" /> is being called.
        ///     This callback is called when a player ends attached object edition mode.
        /// </summary>
        /// <remarks>
        ///     Editions should be discarded if response was '0' (cancelled). This must be done by storing the offsets etc. in an
        ///     array BEFORE using EditAttachedObject.
        /// </remarks>
        public event PlayerEditAttachedObjectHandler EditAttachedObject;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerSelectObject" /> is being called.
        ///     This callback is called when a player selects an object after <see cref="Native.SelectObject" /> has been used.
        /// </summary>
        public event PlayerSelectObjectHandler SelectObject;

        /// <summary>
        ///     Occurs when the <see cref="BaseMode.OnPlayerWeaponShot" /> is being called.
        ///     This callback is called when a player fires a shot from a weapon.
        /// </summary>
        /// <remarks>
        ///     <see cref="BulletHitType.None" />: the fX, fY and fZ parameters are normal coordinates;
        ///     Others: the fX, fY and fZ are offsets from the center of hitid.
        /// </remarks>
        public event WeaponShotHandler WeaponShot;

        #endregion

        #region Players natives

        /// <summary>
        ///     This function can be used to change the spawn information of a specific player. It allows you to automatically set
        ///     someone's spawn weapons, their team, skin and spawn position, normally used in case of minigames or automatic-spawn
        ///     systems. This function is more crash-safe then using <see cref="Native.SetPlayerSkin" /> in
        ///     <see cref="BaseMode.OnPlayerSpawn" /> and/or <see cref="BaseMode.OnPlayerRequestClass" />.
        /// </summary>
        /// <param name="team">The Team-ID of the chosen player.</param>
        /// <param name="skin">The skin which the player will spawn with.</param>
        /// <param name="position">The player's spawn position.</param>
        /// <param name="rotation">The direction in which the player needs to be facing after spawning.</param>
        /// <param name="weapon1">The first spawn-weapon for the player.</param>
        /// <param name="weapon1Ammo">The amount of ammunition for the primary spawnweapon.</param>
        /// <param name="weapon2">The second spawn-weapon for the player.</param>
        /// <param name="weapon2Ammo">The amount of ammunition for the second spawnweapon.</param>
        /// <param name="weapon3">The third spawn-weapon for the player.</param>
        /// <param name="weapon3Ammo">The amount of ammunition for the third spawnweapon.</param>
        public virtual void SetSpawnInfo(int team, int skin, Vector position, float rotation, Weapon weapon1,
            int weapon1Ammo, Weapon weapon2, int weapon2Ammo, Weapon weapon3, int weapon3Ammo)
        {
            Native.SetSpawnInfo(PlayerId, team, skin, position, rotation, weapon1, weapon1Ammo, weapon2, weapon2Ammo,
                weapon3, weapon3Ammo);
        }

        /// <summary>
        ///     (Re)Spawns a player.
        /// </summary>
        public virtual void Spawn()
        {
            Native.SpawnPlayer(PlayerId);
        }

        /// <summary>
        ///     This sets this Player's position then adjusts the Player's z-coordinate to the nearest solid ground under the
        ///     position.
        /// </summary>
        /// <param name="position">The position to move this Player to.</param>
        public virtual void SetPositionFindZ(Vector position)
        {
            Native.SetPlayerPosFindZ(PlayerId, position);
        }

        /// <summary>
        ///     Check if this Player is in range of a point.
        /// </summary>
        /// <param name="range">The furthest distance the player can be from the point to be in range.</param>
        /// <param name="point">The point to check the range to.</param>
        /// <returns>True if this Player is in range of the point, otherwise False.</returns>
        public virtual bool IsInRangeOfPoint(float range, Vector point)
        {
            return Native.IsPlayerInRangeOfPoint(PlayerId, range, point);
        }

        /// <summary>
        ///     Calculate the distance between this Player and a map coordinate.
        /// </summary>
        /// <param name="point">The point to calculate the distance from.</param>
        /// <returns>The distance between the player and the point as a float.</returns>
        public virtual float GetDistanceFromPoint(Vector point)
        {
            return Native.GetPlayerDistanceFromPoint(PlayerId, point);
        }

        /// <summary>
        ///     Checks if a Player is streamed in this Player's client.
        /// </summary>
        /// <remarks>
        ///     Players aren't streamed in on their own client, so if this Player is the same as the other Player, it will return
        ///     false!
        /// </remarks>
        /// <remarks>
        ///     Players stream out if they are more than 150 meters away (see server.cfg - stream_distance)
        /// </remarks>
        /// <param name="other">The Player to check is streamed in.</param>
        /// <returns>True if the other Player is streamed in for this Player, False if not.</returns>
        public virtual bool IsPlayerStreamedIn(Player other)
        {
            return Native.IsPlayerStreamedIn(other.PlayerId, PlayerId);
        }

        /// <summary>
        ///     Set the ammo of this Player's weapon.
        /// </summary>
        /// <param name="weapon">The weapon to set the ammo of.</param>
        /// <param name="ammo">The amount of ammo to set.</param>
        public virtual void SetAmmo(Weapon weapon, int ammo)
        {
            Native.SetPlayerAmmo(PlayerId, (int) weapon, ammo);
        }

        /// <summary>
        ///     Give this Player a Weapon with a specified amount of ammo.
        /// </summary>
        /// <param name="weapon">The Weapon to give to this Player.</param>
        /// <param name="ammo">The amount of ammo to give to this Player.</param>
        public virtual void GiveWeapon(Weapon weapon, int ammo)
        {
            Native.GivePlayerWeapon(PlayerId, (int) weapon, ammo);
        }


        /// <summary>
        ///     Removes all weapons from this Player.
        /// </summary>
        public virtual void ResetWeapons()
        {
            Native.ResetPlayerWeapons(PlayerId);
        }

        /// <summary>
        ///     Sets the armed weapon of this Player.
        /// </summary>
        /// <param name="weapon">The weapon that the player should be armed with.</param>
        public virtual void SetArmedWeapon(Weapon weapon)
        {
            Native.SetPlayerArmedWeapon(PlayerId, (int) weapon);
        }

        /// <summary>
        ///     Get the Weapon and ammo in this Player's weapon slot.
        /// </summary>
        /// <param name="slot">The weapon slot to get data for (0-12).</param>
        /// <param name="weapon">The variable in which to store the weapon, passed by reference.</param>
        /// <param name="ammo">The variable in which to store the ammo, passed by reference.</param>
        public virtual void GetWeaponData(int slot, out Weapon weapon, out int ammo)
        {
            int weaponid;
            Native.GetPlayerWeaponData(PlayerId, slot, out weaponid, out ammo);
            weapon = (Weapon) weaponid;
        }

        /// <summary>
        ///     Give money to this Player.
        /// </summary>
        /// <param name="money">The amount of money to give this Player. Use a minus value to take money.</param>
        public virtual void GiveMoney(int money)
        {
            Native.GivePlayerMoney(PlayerId, money);
        }

        /// <summary>
        ///     Reset this Player's money to $0.
        /// </summary>
        public virtual void ResetMoney()
        {
            Native.ResetPlayerMoney(PlayerId);
        }

        /// <summary>
        ///     Check which keys this Player is pressing.
        /// </summary>
        /// <remarks>
        ///     Only the FUNCTION of keys can be detected; not actual keys. You can not detect if the player presses space, but you
        ///     can detect if they press sprint (which can be mapped (assigned) to ANY key, but is space by default)).
        /// </remarks>
        /// <param name="keys">A set of bits containing this Player's key states</param>
        /// <param name="updown">Up or Down value, passed by reference.</param>
        /// <param name="leftright">Left or Right value, passed by reference.</param>
        public virtual void GetKeys(out Keys keys, out int updown, out int leftright)
        {
            int keysDown;
            Native.GetPlayerKeys(PlayerId, out keysDown, out updown, out leftright);
            keys = (Keys) keysDown;
        }

        /// <summary>
        ///     Sets the clock of this Player to a specific value. This also changes the daytime. (night/day etc.)
        /// </summary>
        /// <param name="hour">Hour to set (0-23).</param>
        /// <param name="minutes">Minutes to set (0-59).</param>
        public virtual void SetTime(int hour, int minutes)
        {
            Native.SetPlayerTime(PlayerId, hour, minutes);
        }

        /// <summary>
        ///     Get this Player's current game time. Set by <see cref="Native.SetWorldTime" />, <see cref="Native.SetWorldTime" />,
        ///     or by <see cref="ToggleClock" />.
        /// </summary>
        /// <param name="hour">The variable to store the hour in, passed by reference.</param>
        /// <param name="minutes">The variable to store the minutes in, passed by reference.</param>
        public virtual void GetTime(out int hour, out int minutes)
        {
            Native.GetPlayerTime(PlayerId, out hour, out minutes);
        }

        /// <summary>
        ///     Show/Hide the in-game clock (top right corner) for this Player.
        /// </summary>
        /// <remarks>
        ///     Time is not synced with other players!
        /// </remarks>
        /// <param name="toggle">True to show, False to hide.</param>
        public virtual void ToggleClock(bool toggle)
        {
            Native.TogglePlayerClock(PlayerId, toggle);
        }

        /// <summary>
        ///     Set this Player's weather. If <see cref="ToggleClock" /> has been used to enable the clock, weather changes will
        ///     interpolate (gradually change), otherwise will change instantly.
        /// </summary>
        /// <param name="weather">The weather to set.</param>
        public virtual void SetWeather(int weather)
        {
            Native.SetPlayerWeather(PlayerId, weather);
        }

        /// <summary>
        ///     Forces this Player to go back to class selection.
        /// </summary>
        /// <remarks>
        ///     The player will not return to class selection until they re-spawn. This can be achieved with
        ///     <see cref="ToggleSpectating" />
        /// </remarks>
        public virtual void ForceClassSelection()
        {
            Native.ForceClassSelection(PlayerId);
        }

        /// <summary>
        ///     This function plays a crime report for this Player - just like in single-player when CJ commits a crime.
        /// </summary>
        /// <param name="suspectid">The ID of the suspect player which will be described in the crime report.</param>
        /// <param name="crime">The crime ID, which will be reported as a 10-code (i.e. 10-16 if 16 was passed as the crimeid).</param>
        public virtual void PlayCrimeReport(int suspectid, int crime)
        {
            Native.PlayCrimeReportForPlayer(PlayerId, suspectid, crime);
        }

        /// <summary>
        ///     Play an 'audio stream' for this Player. Normal audio files also work (e.g. MP3).
        /// </summary>
        /// <param name="url">
        ///     The url to play. Valid formats are mp3 and ogg/vorbis. A link to a .pls (playlist) file will play
        ///     that playlist.
        /// </param>
        /// <param name="position">The position at which to play the audio. Has no effect unless usepos is set to True.</param>
        /// <param name="distance">The distance over which the audio will be heard. Has no effect unless usepos is set to True.</param>
        public virtual void PlayAudioStream(string url, Vector position, float distance)
        {
            Native.PlayAudioStreamForPlayer(PlayerId, url, position.X, position.Y, position.Z, distance, true);
        }

        /// <summary>
        ///     Play an 'audio stream' for this Player. Normal audio files also work (e.g. MP3).
        /// </summary>
        /// <param name="url">
        ///     The url to play. Valid formats are mp3 and ogg/vorbis. A link to a .pls (playlist) file will play
        ///     that playlist.
        /// </param>
        public virtual void PlayAudioStream(string url)
        {
            Native.PlayAudioStreamForPlayer(PlayerId, url, 0, 0, 0, 0, false);
        }

        /// <summary>
        ///     Stops the current audio stream for this Player.
        /// </summary>
        public virtual void StopAudioStream()
        {
            Native.StopAudioStreamForPlayer(PlayerId);
        }

        /// <summary>
        ///     Loads or unloads an interior script for this Player. (for example the ammunation menu)
        /// </summary>
        /// <param name="shopname"></param>
        public virtual void SetShopName(string shopname)
        {
            Native.SetPlayerShopName(PlayerId, shopname);
        }

        /// <summary>
        ///     Set the skill level of a certain weapon type for this Player.
        /// </summary>
        /// <remarks>
        ///     The skill parameter is NOT the weapon ID, it is the skill type.
        /// </remarks>
        /// <param name="skill">The weapon type you want to set the skill of.</param>
        /// <param name="level">
        ///     The skill level to set for that weapon, ranging from 0 to 999. (A level out of range will max it
        ///     out)
        /// </param>
        public virtual void SetSkillLevel(WeaponSkill skill, int level)
        {
            Native.SetPlayerSkillLevel(PlayerId, (int) skill, level);
        }

        /// <summary>
        ///     Removes a standard San Andreas model for this Player within a specified range.
        /// </summary>
        /// <param name="modelid">The model to remove.</param>
        /// <param name="point">The point around which the objects will be removed.</param>
        /// <param name="radius">The radius. Objects within this radius from the coordinates above will be removed.</param>
        public virtual void RemoveBuilding(int modelid, Vector point, float radius)
        {
            Native.RemoveBuildingForPlayer(PlayerId, modelid, point.X, point.Y, point.Z, radius);
        }

        /// <summary>
        ///     Attach an object to a specific bone on this Player.
        /// </summary>
        /// <param name="index">The index (slot) to assign the object to (0-9).</param>
        /// <param name="modelid">The model to attach.</param>
        /// <param name="bone">The bone to attach the object to.</param>
        /// <param name="offset">offset for the object position.</param>
        /// <param name="rotation">rotation of the object.</param>
        /// <param name="scale">scale of the object.</param>
        /// <param name="materialcolor1">The first object color to set, as an integer or hex in ARGB color format.</param>
        /// <param name="materialcolor2">The second object color to set, as an integer or hex in ARGB color format.</param>
        /// <returns>True on success, False otherwise.</returns>
        public virtual bool SetAttachedObject(int index, int modelid, int bone, Vector offset, Vector rotation,
            Vector scale, Color materialcolor1, Color materialcolor2)
        {
            return Native.SetPlayerAttachedObject(PlayerId, index, modelid, bone, offset.X, offset.Y, offset.Z,
                rotation.X, rotation.Y, rotation.Z, scale.X, scale.Y, scale.Z,
                materialcolor1.GetColorValue(ColorFormat.ARGB), materialcolor2.GetColorValue(ColorFormat.ARGB));
        }

        /// <summary>
        ///     Remove an attached object from this Player.
        /// </summary>
        /// <param name="index">The index of the object to remove (set with <see cref="SetAttachedObject" />).</param>
        /// <returns>True on success, False otherwise.</returns>
        public virtual bool RemoveAttachedObject(int index)
        {
            return Native.RemovePlayerAttachedObject(PlayerId, index);
        }

        /// <summary>
        ///     Check if this Player has an object attached in the specified index (slot).
        /// </summary>
        /// <param name="index">The index (slot) to check.</param>
        /// <returns>True if the slot is used, False otherwise.</returns>
        public virtual bool IsAttachedObjectSlotUsed(int index)
        {
            return Native.IsPlayerAttachedObjectSlotUsed(PlayerId, index);
        }

        /// <summary>
        ///     Enter edition mode for an attached object.
        /// </summary>
        /// <param name="index">The index (slot) of the attached object to edit.</param>
        /// <returns>True on success, False otherwise.</returns>
        public virtual bool DoEditAttachedObject(int index)
        {
            return Native.EditAttachedObject(PlayerId, index);
        }

        /// <summary>
        ///     Creates a chat bubble above this Player's name tag.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="color">The text color.</param>
        /// <param name="drawdistance">The distance from where players are able to see the chat bubble.</param>
        /// <param name="expiretime">The time in miliseconds the bubble should be displayed for.</param>
        public virtual void SetChatBubble(string text, Color color, float drawdistance,
            int expiretime)
        {
            Native.SetPlayerChatBubble(PlayerId, text, color.GetColorValue(ColorFormat.RGBA), drawdistance, expiretime);
        }

        /// <summary>
        ///     Puts this Player in a vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle for the player to be put in.</param>
        /// <param name="seatid">The ID of the seat to put the player in.</param>
        public virtual void PutInVehicle(Vehicle vehicle, int seatid)
        {
            Native.PutPlayerInVehicle(PlayerId, vehicle.VehicleId, seatid);
        }

        /// <summary>
        ///     Removes/ejects this Player from his vehicle.
        /// </summary>
        /// <remarks>
        ///     The exiting animation is not synced for other players.
        ///     This function will not work when used in <see cref="BaseMode.OnPlayerEnterVehicle" />, because the player isn't in
        ///     the vehicle when the callback is called. Use <see cref="BaseMode.OnPlayerStateChange" /> instead.
        /// </remarks>
        public virtual void RemoveFromVehicle()
        {
            Native.RemovePlayerFromVehicle(PlayerId);
        }

        /// <summary>
        ///     Toggles whether this Player can control themselves, basically freezes them.
        /// </summary>
        /// <param name="toggle">False to freeze the player or True to unfreeze them.</param>
        public virtual void ToggleControllable(bool toggle)
        {
            Native.TogglePlayerControllable(PlayerId, toggle);
        }

        /// <summary>
        ///     Plays the specified sound for this Player at a specific point.
        /// </summary>
        /// <param name="soundid">The sound to play.</param>
        /// <param name="point">Point for the sound to play at.</param>
        public virtual void PlaySound(int soundid, Vector point)
        {
            Native.PlayerPlaySound(PlayerId, soundid, point.X, point.Y, point.Z);
        }

        /// <summary>
        ///     Plays the specified sound for this Player.
        /// </summary>
        /// <param name="soundid">The sound to play.</param>
        public virtual void PlaySound(int soundid)
        {
            Native.PlayerPlaySound(PlayerId, soundid, 0, 0, 0);
        }

        /// <summary>
        ///     Apply an animation to this Player.
        /// </summary>
        /// <remarks>
        ///     The <paramref name="forcesync" /> parameter, in most cases is not needed since players sync animations themselves.
        ///     The <paramref name="forcesync" /> parameter can force all players who can see this Player to play the animation
        ///     regardless of whether the player is performing that animation. This is useful in circumstances where the player
        ///     can't sync the animation themselves. For example, they may be paused.
        /// </remarks>
        /// <param name="animlib">The name of the animation library in which the animation to apply is in.</param>
        /// <param name="animname">The name of the animation, within the library specified.</param>
        /// <param name="fDelta">The speed to play the animation (use 4.1).</param>
        /// <param name="loop">Set to True for looping otherwise set to False for playing animation sequence only once.</param>
        /// <param name="lockx">
        ///     Set to False to return player to original x position after animation is complete for moving
        ///     animations. The opposite effect occurs if set to True.
        /// </param>
        /// <param name="locky">
        ///     Set to False to return player to original y position after animation is complete for moving
        ///     animations. The opposite effect occurs if set to True.
        /// </param>
        /// <param name="freeze">Will freeze the player in position after the animation finishes.</param>
        /// <param name="time">Timer in milliseconds. For a never ending loop it should be 0.</param>
        /// <param name="forcesync">Set to True to force playerid to sync animation with other players in all instances</param>
        public virtual void ApplyAnimation(string animlib, string animname, float fDelta, bool loop, bool lockx,
            bool locky, bool freeze, int time, bool forcesync)
        {
            Native.ApplyAnimation(PlayerId, animlib, animname, fDelta, loop, lockx, locky, freeze, time, forcesync);
        }

        /// <summary>
        ///     Apply an animation to this Player.
        /// </summary>
        /// <param name="animlib">The name of the animation library in which the animation to apply is in.</param>
        /// <param name="animname">The name of the animation, within the library specified.</param>
        /// <param name="fDelta">The speed to play the animation (use 4.1).</param>
        /// <param name="loop">Set to True for looping otherwise set to False for playing animation sequence only once.</param>
        /// <param name="lockx">
        ///     Set to False to return player to original x position after animation is complete for moving
        ///     animations. The opposite effect occurs if set to True.
        /// </param>
        /// <param name="locky">
        ///     Set to False to return player to original y position after animation is complete for moving
        ///     animations. The opposite effect occurs if set to True.
        /// </param>
        /// <param name="freeze">Will freeze the player in position after the animation finishes.</param>
        /// <param name="time">Timer in milliseconds. For a never ending loop it should be 0.</param>
        public virtual void ApplyAnimation(string animlib, string animname, float fDelta, bool loop, bool lockx,
            bool locky, bool freeze, int time)
        {
            Native.ApplyAnimation(PlayerId, animlib, animname, fDelta, loop, lockx, locky, freeze, time, false);
        }

        /// <summary>
        ///     Clears all animations for this Player.
        /// </summary>
        /// <param name="forcesync">Specifies whether the animation should be shown to streamed in players.</param>
        public virtual void ClearAnimations(bool forcesync)
        {
            Native.ClearAnimations(PlayerId, forcesync);
        }

        /// <summary>
        ///     Clears all animations for this Player.
        /// </summary>
        public virtual void ClearAnimations()
        {
            Native.ClearAnimations(PlayerId, false);
        }

        /// <summary>
        ///     Get the animation library/name this Player is playing.
        /// </summary>
        /// <param name="animlib">String variable that stores the animation library.</param>
        /// <param name="animname">String variable that stores the animation name.</param>
        /// <returns>True on success, False otherwise.</returns>
        public virtual bool GetAnimationName(out string animlib, out string animname)
        {
            return Native.GetAnimationName(AnimationIndex, out animlib, 64, out animname, 64);
        }

        /// <summary>
        ///     Sets a checkpoint (red circle) for this Player. Also shows a red blip on the radar.
        /// </summary>
        /// <remarks>
        ///     Checkpoints created on server-created objects will appear down on the 'real' ground, but will still function correctly.
        ///     There is no fix available for this issue. A pickup can be used instead.
        /// </remarks>
        /// <param name="point">The point to set the checkpoint at.</param>
        /// <param name="size">The size of the checkpoint.</param>
        public virtual void SetCheckpoint(Vector point, float size)
        {
            Native.SetPlayerCheckpoint(PlayerId, point, size);
        }

        /// <summary>
        ///     Disable any initialized checkpoints for this Player.
        /// </summary>
        public virtual void DisableCheckpoint()
        {
            Native.DisablePlayerCheckpoint(PlayerId);
        }

        /// <summary>
        ///     Creates a race checkpoint. When this Player enters it, the <see cref="EnterRaceCheckpoint" /> callback is called.
        /// </summary>
        /// <param name="type">Type of checkpoint.</param>
        /// <param name="point">The point to set the checkpoint at.</param>
        /// <param name="nextPosition">Coordinates of the next point, for the arrow facing direction.</param>
        /// <param name="size">Size (diameter) of the checkpoint</param>
        public virtual void SetRaceCheckpoint(CheckpointType type, Vector point, Vector nextPosition, float size)
        {
            Native.SetPlayerRaceCheckpoint(PlayerId, type, point, nextPosition, size);
        }

        /// <summary>
        ///     Disable any initialized race checkpoints for this Player.
        /// </summary>
        public virtual void DisableRaceCheckpoint()
        {
            Native.DisablePlayerRaceCheckpoint(PlayerId);
        }

        /// <summary>
        ///     Set the world boundaries for this Player - players can not go out of the boundaries.
        /// </summary>
        /// <remarks>
        ///     You can reset the player world bounds by setting the parameters to 20000.0000, -20000.0000, 20000.0000,
        ///     -20000.0000.
        /// </remarks>
        /// <param name="xMax">The maximum X coordinate the player can go to.</param>
        /// <param name="xMin">The minimum X coordinate the player can go to.</param>
        /// <param name="yMax">The maximum Y coordinate the player can go to.</param>
        /// <param name="yMin">The minimum Y coordinate the player can go to.</param>
        public virtual void SetWorldBounds(float xMax, float xMin, float yMax, float yMin)
        {
            Native.SetPlayerWorldBounds(PlayerId, xMax, xMin, yMax, yMin);
        }

        /// <summary>
        ///     Change the colour of this Player's nametag and radar blip for another Player.
        /// </summary>
        /// <param name="player">The player whose color will be changed.</param>
        /// <param name="color">New color.</param>
        public virtual void SetMarkerForPlayer(Player player, Color color)
        {
            Native.SetPlayerMarkerForPlayer(PlayerId, player.PlayerId, color.GetColorValue(ColorFormat.RGBA));
        }

        /// <summary>
        ///     This functions allows you to toggle the drawing of player nametags, healthbars and armor bars which display above
        ///     their head. For use of a similar function like this on a global level, <see cref="Native.ShowNameTags" /> function.
        /// </summary>
        /// <remarks>
        ///     <see cref="Native.ShowNameTags" /> must be set to True to be able to show name tags with
        ///     <see cref="ShowNameTagForPlayer" />.
        /// </remarks>
        /// <param name="player">Player whose name tag will be shown or hidden.</param>
        /// <param name="show">True to show name tag, False to hide name tag.</param>
        public virtual void ShowNameTagForPlayer(Player player, bool show)
        {
            Native.ShowPlayerNameTagForPlayer(PlayerId, player.PlayerId, show);
        }

        /// <summary>
        ///     This function allows you to place your own icons on the map, enabling you to emphasise the locations of banks,
        ///     airports or whatever else you want. A total of 63 icons are available in GTA: San Andreas, all of which can be used
        ///     using this function. You can also specify the color of the icon, which allows you to change the square icon (ID:
        ///     0).
        /// </summary>
        /// <param name="iconid">The player's icon ID, ranging from 0 to 99, to be used in RemovePlayerMapIcon.</param>
        /// <param name="position">The coordinates of the place where you want the icon to be.</param>
        /// <param name="markertype">The icon to set.</param>
        /// <param name="color">The color of the icon, this should only be used with the square icon (ID: 0).</param>
        /// <param name="style">The style of icon.</param>
        /// <returns>True if it was successful, False otherwise (e.g. the player isn't connected).</returns>
        public virtual bool SetMapIcon(int iconid, Vector position, PlayerMarkersMode markertype, Color color,
            MapIconType style)
        {
            return Native.SetPlayerMapIcon(PlayerId, iconid, position, markertype, color, (int) style);
        }

        /// <summary>
        ///     Removes a map icon that was set earlier for this Player.
        /// </summary>
        /// <param name="iconid">The ID of the icon to remove. This is the second parameter of <see cref="SetMapIcon" />.</param>
        public virtual void RemovePlayerMapIcon(int iconid)
        {
            Native.RemovePlayerMapIcon(PlayerId, iconid);
        }

        /// <summary>
        ///     Set the direction this Player's camera looks at. To be used in combination with SetPlayerCameraPos.
        /// </summary>
        /// <param name="point">The coordinates for this Player's camera to look at.</param>
        /// <param name="cut">The style the camera-position changes.</param>
        public virtual void SetCameraLookAt(Vector point, CameraCut cut)
        {
            Native.SetPlayerCameraLookAt(PlayerId, point, cut);
        }

        /// <summary>
        ///     You can use this function to attach this Player camera to objects.
        /// </summary>
        /// <remarks>
        ///     You need to create the object first, before attempting to attach a player camera for that.
        /// </remarks>
        /// <param name="objectid">The object id which you want to attach the player camera.</param>
        /// <returns>This function doesn't return a specific value.</returns>
        public virtual void AttachCameraToObject(int objectid)
        {
            Native.AttachCameraToObject(PlayerId, objectid);
        }

        /// <summary>
        ///     Attaches this Player's camera to a player-object. They are able to move their camera while it is attached to an
        ///     object.
        /// </summary>
        /// <param name="playerobjectid">The ID of the player-object to which the player's camera will be attached.</param>
        public virtual void AttachCameraToPlayerObject(int playerobjectid)
        {
            Native.AttachCameraToPlayerObject(PlayerId, playerobjectid);
        }

        /// <summary>
        ///     Move this Player's camera from one position to another, within the set time.
        /// </summary>
        /// <param name="from">The position the camera should start to move from.</param>
        /// <param name="to">The position the camera should move to.</param>
        /// <param name="time">Time in milliseconds.</param>
        /// <param name="cut">The jumpcut to use. Defaults to CameraCut.Cut. Set to CameraCut. Move for a smooth movement.</param>
        public virtual void InterpolateCameraPos(Vector from, Vector to, int time, CameraCut cut)
        {
            Native.InterpolateCameraPos(PlayerId, from, to, time, cut);
        }

        /// <summary>
        ///     Interpolate this Player's camera's 'look at' point between two coordinates with a set speed.
        /// </summary>
        /// <param name="from">The position the camera should start to move from.</param>
        /// <param name="to">The position the camera should move to.</param>
        /// <param name="time">Time in milliseconds to complete interpolation.</param>
        /// <param name="cut">The 'jumpcut' to use. Defaults to CameraCut.Cut (pointless). Set to CameraCut.Move for interpolation.</param>
        public virtual void InterpolateCameraLookAt(Vector from, Vector to, int time, CameraCut cut)
        {
            Native.InterpolateCameraLookAt(PlayerId, from, to, time, cut);
        }

        /// <summary>
        ///     Checks if this Player is in a specific vehicle.
        /// </summary>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns>True if player is in the vehicle, otherwise False.</returns>
        public virtual bool IsInVehicle(Vehicle vehicle)
        {
            return Native.IsPlayerInVehicle(PlayerId, vehicle.VehicleId);
        }

        /// <summary>
        ///     Toggle stunt bonuses for this Player.
        /// </summary>
        /// <param name="enable">True to enable stunt bonuses, False to disable them.</param>
        public virtual void EnableStuntBonus(bool enable)
        {
            Native.EnableStuntBonusForPlayer(PlayerId, enable);
        }

        /// <summary>
        ///     Toggle this Player's spectate mode.
        /// </summary>
        /// <remarks>
        ///     When the spectating is turned off, OnPlayerSpawn will automatically be called.
        /// </remarks>
        /// <param name="toggle">True to enable spectating and False to disable.</param>
        public virtual void ToggleSpectating(bool toggle)
        {
            Native.TogglePlayerSpectating(PlayerId, toggle);
        }

        /// <summary>
        ///     Makes this Player spectate (watch) another player.
        /// </summary>
        /// <remarks>
        ///     Order is CRITICAL! Ensure that you use <see cref="ToggleSpectating" /> before <see cref="SpectatePlayer" />.
        /// </remarks>
        /// <param name="targetplayerid">The ID of the player that should be spectated.</param>
        /// <param name="mode">The mode to spectate with.</param>
        public virtual void SpectatePlayer(int targetplayerid, SpectateMode mode)
        {
            Native.PlayerSpectatePlayer(PlayerId, targetplayerid, (int) mode);
        }

        /// <summary>
        ///     Sets this Player to spectate another vehicle, i.e. see what its driver sees.
        /// </summary>
        /// <remarks>
        ///     Order is CRITICAL! Ensure that you use <see cref="ToggleSpectating" /> before <see cref="SpectateVehicle" />.
        /// </remarks>
        /// <param name="targetvehicle">The vehicle to spectate.</param>
        /// <param name="mode">Spectate mode.</param>
        /// <returns>This function doesn't return a specific value.</returns>
        public virtual void SpectateVehicle(Vehicle targetvehicle, SpectateMode mode)
        {
            Native.PlayerSpectateVehicle(PlayerId, targetvehicle.VehicleId, (int) mode);
        }

        /// <summary>
        ///     Returns the ID of the object this Player is surfing on.
        /// </summary>
        /// <returns>
        ///     The ID of the moving object the player is surfing. If the player isn't surfing a moving object, it will return
        ///     <see cref="Misc.InvalidObjectId" />
        /// </returns>
        public virtual int GetSurfingObjectID()
        {
            return Native.GetPlayerSurfingObjectID(PlayerId);
        }

        /// <summary>
        ///     Starts recording this Player's movements to a file, which can then be reproduced by an NPC.
        /// </summary>
        /// <param name="recordtype">The type of recording.</param>
        /// <param name="recordname">
        ///     Name of the file which will hold the recorded data. It will be saved in scriptfiles, with an
        ///     automatically added .rec extension.
        /// </param>
        public virtual void StartRecordingPlayerData(PlayerRecordingType recordtype, string recordname)
        {
            Native.StartRecordingPlayerData(PlayerId, (int) recordtype, recordname);
        }

        /// <summary>
        ///     Stops all the recordings that had been started with <see cref="StartRecordingPlayerData" /> for this Player.
        /// </summary>
        public virtual void StopRecordingPlayerData()
        {
            Native.StopRecordingPlayerData(PlayerId);
        }

        #endregion

        #region SAMP natives

        /// <summary>
        ///     This function sends a message to this Player with a chosen color in the chat. The whole line in the chatbox will be
        ///     in the set color unless colour embedding is used.<br />
        /// </summary>
        /// <param name="color">The color of the message.</param>
        /// <param name="message">The text that will be displayed (max 144 characters).</param>
        public void SendClientMessage(Color color, string message)
        {
            Native.SendClientMessage(PlayerId, color.GetColorValue(ColorFormat.RGBA), message);
        }

        /// <summary>
        ///     Displays a message in chat to all players. This is a multi-player equivalent of <see cref="SendClientMessage" />.
        ///     <br />
        /// </summary>
        /// <param name="color">The color of the message (RGBA Hex format).</param>
        /// <param name="message">The message to show (max 144 characters).</param>
        public static void SendClientMessageToAll(Color color, string message)
        {
            Native.SendClientMessageToAll(color.GetColorValue(ColorFormat.RGBA), message);
        }

        /// <summary>
        ///     Sends a message in the name this Player to another player on the server. The message will appear in the chat box
        ///     but can only be seen by <paramref name="receiver" />. The line will start with the this Player's name in his color,
        ///     followed by the <paramref name="message" /> in white.
        /// </summary>
        /// <param name="receiver">The Player who will recieve the message</param>
        /// <param name="message">The message that will be sent.</param>
        public void SendPlayerMessageToPlayer(Player receiver, string message)
        {
            Native.SendPlayerMessageToPlayer(receiver.PlayerId, PlayerId, message);
        }

        /// <summary>
        ///     Sends a message in the name of this Player to all other players on the server. The line will start with the this
        ///     Player's name in their color, followed by the <paramref name="message" /> in white.
        /// </summary>
        /// <param name="message">The message that will be sent.</param>
        public void SendPlayerMessageToAll(string message)
        {
            Native.SendPlayerMessageToAll(PlayerId, message);
        }

        /// <summary>
        ///     Adds a death to the 'killfeed' on the right-hand side of the screen.
        /// </summary>
        /// <param name="killer">The Player that killer this Player.</param>
        /// <param name="weapon">
        ///     The reason (not always a weapon) for this Player's death. Special icons can also be used
        ///     (ICON_CONNECT and ICON_DISCONNECT).
        /// </param>
        public void SendDeathMessage(Player killer, Weapon weapon)
        {
            Native.SendDeathMessage(killer == null ? InvalidId : killer.PlayerId, PlayerId, (int) weapon);
        }

        /// <summary>
        ///     Shows 'game text' (on-screen text) for a certain length of time for all players.
        /// </summary>
        /// <param name="text">The text to be displayed.</param>
        /// <param name="time">The duration of the text being shown in milliseconds.</param>
        /// <param name="style">The style of text to be displayed.</param>
        public static void GameTextForAll(string text, int time, int style)
        {
            Native.GameTextForAll(text, time, style);
        }

        /// <summary>
        ///     Shows 'game text' (on-screen text) for a certain length of time for this Player.
        /// </summary>
        /// <param name="text">The text to be displayed.</param>
        /// <param name="time">The duration of the text being shown in milliseconds.</param>
        /// <param name="style">The style of text to be displayed.</param>
        public void GameText(string text, int time, int style)
        {
            Native.GameTextForPlayer(PlayerId, text, time, style);
        }

        #endregion

        #region Event raisers

        /// <summary>
        ///     Raises the <see cref="Connected" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEventArgs" /> that contains the event data. </param>
        public virtual void OnConnected(PlayerEventArgs e)
        {
            if (Connected != null)
                Connected(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="Disconnected" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerDisconnectedEventArgs" /> that contains the event data. </param>
        public virtual void OnDisconnected(PlayerDisconnectedEventArgs e)
        {
            if (Disconnected != null)
                Disconnected(this, e);

            Dispose();
        }

        /// <summary>
        ///     Raises the <see cref="Spawned" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEventArgs" /> that contains the event data. </param>
        public virtual void OnSpawned(PlayerEventArgs e)
        {
            if (Spawned != null)
                Spawned(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="Died" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerDeathEventArgs" /> that contains the event data. </param>
        public virtual void OnDeath(PlayerDeathEventArgs e)
        {
            if (Died != null)
                Died(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="Text" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerTextEventArgs" /> that contains the event data. </param>
        public virtual void OnText(PlayerTextEventArgs e)
        {
            if (Text != null)
                Text(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="CommandText" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerTextEventArgs" /> that contains the event data. </param>
        public virtual void OnCommandText(PlayerTextEventArgs e)
        {
            if (CommandText != null)
                CommandText(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="RequestClass" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerRequestClassEventArgs" /> that contains the event data. </param>
        public virtual void OnRequestClass(PlayerRequestClassEventArgs e)
        {
            if (RequestClass != null)
                RequestClass(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="EnterVehicle" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEnterVehicleEventArgs" /> that contains the event data. </param>
        public virtual void OnEnterVehicle(PlayerEnterVehicleEventArgs e)
        {
            if (EnterVehicle != null)
                EnterVehicle(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="ExitVehicle" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerVehicleEventArgs" /> that contains the event data. </param>
        public virtual void OnExitVehicle(PlayerVehicleEventArgs e)
        {
            if (ExitVehicle != null)
                ExitVehicle(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="StateChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerStateEventArgs" /> that contains the event data. </param>
        public virtual void OnStateChanged(PlayerStateEventArgs e)
        {
            if (StateChanged != null)
                StateChanged(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="EnterCheckpoint" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEventArgs" /> that contains the event data. </param>
        public virtual void OnEnterCheckpoint(PlayerEventArgs e)
        {
            if (EnterCheckpoint != null)
                EnterCheckpoint(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="LeaveCheckpoint" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEventArgs" /> that contains the event data. </param>
        public virtual void OnLeaveCheckpoint(PlayerEventArgs e)
        {
            if (LeaveCheckpoint != null)
                LeaveCheckpoint(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="EnterRaceCheckpoint" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEventArgs" /> that contains the event data. </param>
        public virtual void OnEnterRaceCheckpoint(PlayerEventArgs e)
        {
            if (EnterRaceCheckpoint != null)
                EnterRaceCheckpoint(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="LeaveRaceCheckpoint" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEventArgs" /> that contains the event data. </param>
        public virtual void OnLeaveRaceCheckpoint(PlayerEventArgs e)
        {
            if (LeaveRaceCheckpoint != null)
                LeaveRaceCheckpoint(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="RequestSpawn" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEventArgs" /> that contains the event data. </param>
        public virtual void OnRequestSpawn(PlayerEventArgs e)
        {
            if (RequestSpawn != null)
                RequestSpawn(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="PickUpPickup" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerPickupEventArgs" /> that contains the event data. </param>
        public virtual void OnPickUpPickup(PlayerPickupEventArgs e)
        {
            if (PickUpPickup != null)
                PickUpPickup(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="EnterExitModShop" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEnterModShopEventArgs" /> that contains the event data. </param>
        public virtual void OnEnterExitModShop(PlayerEnterModShopEventArgs e)
        {
            if (EnterExitModShop != null)
                EnterExitModShop(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="SelectedMenuRow" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerSelectedMenuRowEventArgs" /> that contains the event data. </param>
        public virtual void OnSelectedMenuRow(PlayerSelectedMenuRowEventArgs e)
        {
            if (SelectedMenuRow != null)
                SelectedMenuRow(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="ExitedMenu" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEventArgs" /> that contains the event data. </param>
        public virtual void OnExitedMenu(PlayerEventArgs e)
        {
            if (ExitedMenu != null)
                ExitedMenu(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="InteriorChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerInteriorChangedEventArgs" /> that contains the event data. </param>
        public virtual void OnInteriorChanged(PlayerInteriorChangedEventArgs e)
        {
            if (InteriorChanged != null)
                InteriorChanged(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="KeyStateChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerKeyStateChangedEventArgs" /> that contains the event data. </param>
        public virtual void OnKeyStateChanged(PlayerKeyStateChangedEventArgs e)
        {
            if (KeyStateChanged != null)
                KeyStateChanged(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="Update" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEventArgs" /> that contains the event data. </param>
        public virtual void OnUpdate(PlayerEventArgs e)
        {
            if (Update != null)
                Update(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="StreamIn" /> event.
        /// </summary>
        /// <param name="e">An <see cref="StreamPlayerEventArgs" /> that contains the event data. </param>
        public virtual void OnStreamIn(StreamPlayerEventArgs e)
        {
            if (StreamIn != null)
                StreamIn(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="StreamOut" /> event.
        /// </summary>
        /// <param name="e">An <see cref="StreamPlayerEventArgs" /> that contains the event data. </param>
        public virtual void OnStreamOut(StreamPlayerEventArgs e)
        {
            if (StreamOut != null)
                StreamOut(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="DialogResponse" /> event.
        /// </summary>
        /// <param name="e">An <see cref="DialogResponseEventArgs" /> that contains the event data. </param>
        public virtual void OnDialogResponse(DialogResponseEventArgs e)
        {
            if (DialogResponse != null)
                DialogResponse(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="TakeDamage" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerDamageEventArgs" /> that contains the event data. </param>
        public virtual void OnTakeDamage(PlayerDamageEventArgs e)
        {
            if (TakeDamage != null)
                TakeDamage(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="GiveDamage" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerDamageEventArgs" /> that contains the event data. </param>
        public virtual void OnGiveDamage(PlayerDamageEventArgs e)
        {
            if (GiveDamage != null)
                GiveDamage(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="ClickMap" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerClickMapEventArgs" /> that contains the event data. </param>
        public virtual void OnClickMap(PlayerClickMapEventArgs e)
        {
            if (ClickMap != null)
                ClickMap(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="ClickTextDraw" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerClickTextDrawEventArgs" /> that contains the event data. </param>
        public virtual void OnClickTextDraw(PlayerClickTextDrawEventArgs e)
        {
            if (ClickTextDraw != null)
                ClickTextDraw(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="ClickPlayerTextDraw" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerClickTextDrawEventArgs" /> that contains the event data. </param>
        public virtual void OnClickPlayerTextDraw(PlayerClickTextDrawEventArgs e)
        {
            if (ClickPlayerTextDraw != null)
                ClickPlayerTextDraw(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="ClickPlayer" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerClickPlayerEventArgs" /> that contains the event data. </param>
        public virtual void OnClickPlayer(PlayerClickPlayerEventArgs e)
        {
            if (ClickPlayer != null)
                ClickPlayer(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="EditObject" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEditObjectEventArgs" /> that contains the event data. </param>
        public virtual void OnEditObject(PlayerEditObjectEventArgs e)
        {
            if (EditObject != null)
                EditObject(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="EditAttachedObject" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerEditAttachedObjectEventArgs" /> that contains the event data. </param>
        public virtual void OnEditAttachedObject(PlayerEditAttachedObjectEventArgs e)
        {
            if (EditAttachedObject != null)
                EditAttachedObject(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="SelectObject" /> event.
        /// </summary>
        /// <param name="e">An <see cref="PlayerSelectObjectEventArgs" /> that contains the event data. </param>
        public virtual void OnSelectObject(PlayerSelectObjectEventArgs e)
        {
            if (SelectObject != null)
                SelectObject(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="WeaponShot" /> event.
        /// </summary>
        /// <param name="e">An <see cref="WeaponShotEventArgs" /> that contains the event data. </param>
        public virtual void OnWeaponShot(WeaponShotEventArgs e)
        {
            if (WeaponShot != null)
                WeaponShot(this, e);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Removes this Player from memory. It is best to dispose the object when the player has disconnected.
        /// </summary>
        public virtual void Dispose()
        {
            Instances.Remove(this);
        }

        public override int GetHashCode()
        {
            return PlayerId;
        }

        public override string ToString()
        {
            return string.Format("Player(Id:{0}, Name:{1})", PlayerId, Name);
        }

        #endregion
    }
}