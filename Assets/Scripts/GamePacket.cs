using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Position {
    public float x;
    public float y;
    public float z;

    public Position(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

public class GamePacket
{    
    /* Three types of events :
        gameStatus update (start or end), 1 , 2
        playerState update (hp or position), 3, 4
        attack update 5
    */
    public string gameEvent { get; set; }
    public string info { get; set; }


    public string playerID { get; set; }
    public string time { get; set; }
    // player information
    public Position playerPosition;
    public int p_HP { get; set;}
       
     // attack information
    public Position attackPosition;

    public GamePacket () {
        gameEvent = "";
        info = "";
        playerID = "sudoID";
        p_HP = 5;
        time = System.DateTime.Now.ToString("HH:mm:ss");
        playerPosition = new Position(0,0,0);
        attackPosition = new Position(0,0,0);
    }

    public ByteBuffer GetBuffer() {
        ByteBuffer buffer = new ByteBuffer();
        buffer.WriteString(gameEvent);
        buffer.WriteString(info);

        buffer.WriteString(playerID);
        buffer.WriteInteger(p_HP);

        time = System.DateTime.Now.ToString("HH:mm:ss");
        buffer.WriteString(time);

        buffer.WriteFloat(playerPosition.x);
        buffer.WriteFloat(playerPosition.y);
        buffer.WriteFloat(playerPosition.z);
        buffer.WriteFloat(attackPosition.x);
        buffer.WriteFloat(attackPosition.x);
        buffer.WriteFloat(attackPosition.x);
        return buffer;
    }

    public void GeneratePacket(byte[] buffer, bool print = false) {
        // generate packet data from bytes
        ByteBuffer byteBuffer = new ByteBuffer();
        byteBuffer.WriteBytes(buffer);

        gameEvent = byteBuffer.ReadString();
        info = byteBuffer.ReadString();

        playerID = byteBuffer.ReadString();
        p_HP = byteBuffer.ReadInteger();

        time = byteBuffer.ReadString();

        float x,y,z;

        x = byteBuffer.ReadFloat();
        y = byteBuffer.ReadFloat();
        z = byteBuffer.ReadFloat();
        playerPosition = new Position(x,y,z);

        x = byteBuffer.ReadFloat();
        y = byteBuffer.ReadFloat();
        z = byteBuffer.ReadFloat();
        attackPosition = new Position(x,y,z);

        if (print) {
            debugMsg();
        }
    }

    private void debugMsg() {
        Debug.Log(
            "gameEvent:" + gameEvent 
            + ", info:" + info 
            + ", playerID:" + playerID
            + ", p_HP:" + p_HP
            + ", time:" + time
            + ", playerPosition:(" + playerPosition.x + ", " + playerPosition.y + ", " + playerPosition.z + ")"
            + ", attackPosition:(" + attackPosition.x + ", " +attackPosition.y + ", " + attackPosition.z + ")");
        
    }
};
