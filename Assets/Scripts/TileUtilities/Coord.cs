﻿using UnityEngine;

public struct Coord {

    public int x;
    public int y;

    public Coord(int x, int y) {
        this.x = x;
        this.y = y;
    }




    //Constants
    public static Coord origin {
        get {
            return new Coord(0, 0);
        }
    }



    // Operations
    public static bool operator ==(Coord a, Coord b) {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Coord a, Coord b) {
        return a.x != b.x || a.y != b.y;
    }

    public override bool Equals(object obj) {
        return (Coord)obj == this;
    }

    public override int GetHashCode() {
        return 0;
    }

    public override string ToString() {
        return "(" + x + " , " + y + ")";
    }
}
