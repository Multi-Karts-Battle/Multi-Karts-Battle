using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Position {
    int x;
    int y;
    int z;

    public Position(int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

public class GameState 
{
    public string playerID { get; set; }
    public System.DateTime time { get; set; }
    
    /* Three types of events :
        gameStatus update (start or end),
        playerState update (hp or position), 
        attack update
    */
    public string gameEvent { get; set; }
    public string info { get; set; }

    // player information
    public Position playerPosition;
    public int p_HP { get; set;}
       
     // attack information
    public Position attackPosition;

    public ByteBuffer GetBuffer() {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteString(gameEvent);
        buffer.WriteString(playerID);
        buffer.WriteString(info);
        return buffer;
    }
};