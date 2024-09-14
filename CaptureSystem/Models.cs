using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
using Rocket.Unturned.Player;
using SDG.Unturned;

namespace CaptureSystem
{
    public class CaptureSystem
    {
        [XmlArray("Location")]
        public List<Location> Location = new List<Location>();

        [XmlArray("Transport")]
        public List<Transport> Transport = new List<Transport>();

        [XmlArray("Hangar")]
        public List<Hangar> Hangar = new List<Hangar>();

        [XmlArray("TransportInHangar")]
        public List<TransportInHangar> TransportInHangar = new List<TransportInHangar>();

        [XmlArray("TransportImage")]
        public List<TransportImage> TransportImage = new List<TransportImage>();

        [XmlArray("PlayerInf")]
        public List<PlayerInf> PlayerInf = new List<PlayerInf>();

        [XmlArray("Team")]
        public List<Team> Team = new List<Team>();

        [XmlArray("Rank")]
        public List<Rank> Rank = new List<Rank>();

        [XmlArray("Fraction")]
        public List<Fraction> Fraction = new List<Fraction>();

        [XmlArray("Subclass")]
        public List<Subclass> Subclass = new List<Subclass>();

        public CaptureSystem() { }
    }


    public class Location
    {
        public string id;

        public string name;

        public string team;

        public Vector3 point;

        public float radius;

        public List<Boost> boosts;

        public Location() { }

        public Location(string id, string name, string team, Vector3 point, float radius, List<Boost> boosts)
        {
            this.id = id;
            this.name = name;
            this.team = team;
            this.point = point;
            this.radius = radius;
            this.boosts = boosts;
        }
    }


    public class Boost
    {
        public ushort teamA;

        public ushort teamB;

        public int amount_storage;

        public int supply;

        public Boost() { }

        public Boost(ushort teamA, ushort teamB, int amount_storage, int supply)
        {
            this.teamA = teamA;
            this.teamB = teamB;
            this.amount_storage = amount_storage;
            this.supply = supply;
        }
    }


    public class ItemAndAmount
    {
        public ushort id;

        public int amount;

        public ItemAndAmount() { }

        public ItemAndAmount(ushort id, int amount)
        {
            this.id = id;
            this.amount = amount;
        }
    }


    public class Transport
    {
        public ushort id;

        public string name;

        public string classification;

        public int kd;

        public int rang;

        public List<ItemAndAmount> set;

        public Transport() { }

        public Transport(ushort id, string name, string classification, int kd, int rang, List<ItemAndAmount> set)
        {
            this.id = id;
            this.name = name;
            this.classification = classification;
            this.kd = kd;
            this.rang = rang;
            this.set = set;
        }
        public void GetItems(UnturnedPlayer player)
        {
            foreach (var i in set)
            {
                for (var j = 0; j < i.amount; j++)
                {
                    Item item = new Item(i.id, true);
                    player.Inventory.forceAddItem(item, true);
                }
            }
        }
    }


    public class TransportImage
    {
        public ushort id;

        public string url_image;

        public TransportImage() { }

        public TransportImage(ushort id, string url_image)
        {
            this.id = id;
            this.url_image = url_image;
        }
    }


    public class PointSpawnTransport
    {
        public string classification;

        public Vector3 point;

        public Quaternion quaternion;

        public PointSpawnTransport() { }

        public PointSpawnTransport(string classification, Vector3 point, Quaternion quaternion)
        {
            this.classification = classification;
            this.point = point;
            this.quaternion = quaternion;
        }
    }


    public class TransportInHangar
    {
        public ushort id;

        public string team;

        public int amount_base;

        public int amount;

        public int supply;

        public int existence;

        public TransportInHangar() { }

        public TransportInHangar(ushort id, string team, int amount_base, int amount, int supply, int existence)
        {
            this.id = id;
            this.team = team;
            this.amount_base = amount_base;
            this.amount = amount;
            this.supply = supply;
            this.existence = existence;
        }
    }


    public class Hangar
    {
        public string id;

        public string name;

        public float radius;

        public Vector3 point;

        public List<PointSpawnTransport> pointSpawnTransports;

        public List<TransportInHangar> transportInHangars;
        public Hangar() { }

        public Hangar(string id, string name, float radius, Vector3 point,
                        List<PointSpawnTransport> pointSpawnTransports,
                        List<TransportInHangar> transportInHangars)
        {
            this.id = id;
            this.name = name;
            this.radius = radius;
            this.point = point;
            this.pointSpawnTransports = pointSpawnTransports;
            this.transportInHangars = transportInHangars;
        }
    }


    public class PlayerInf
    {
        public string team;

        public int score;

        public int rang;

        public Steamworks.CSteamID player;

        public PlayerInf() { }

        public PlayerInf(string team, int score, int rang, Steamworks.CSteamID player)
        {
            this.team = team;
            this.score = score;
            this.rang = rang;
            this.player = player;
        }
    }


    public class Team
    {
        public string id;

        public string name;

        public Vector3 point;

        public Steamworks.CSteamID group_id;

        public Team() { }

        public Team(string id, string name, Vector3 point, Steamworks.CSteamID group_id)
        {
            this.id = id;
            this.name = name;
            this.point = point;
            this.group_id = group_id;
        }
    }


    public class Rank
    {
        public string name;

        public int score;

        public int pk;

        public Rank() { }
        public Rank(string name, int score, int pk)
        {
            this.name = name;
            this.score = score;
            this.pk = pk;
        }
    }

    public class Fraction
    {
        public string id;

        public string team;

        public string name;

        public int need_rank;

        public string image;

        public Fraction() { }

        public Fraction(string id, string team,  string name, int need_rank, string image)
        {
            this.id = id;
            this.team = team;
            this.name = name;
            this.need_rank = need_rank;
            this.image = image;
        }
    }

    public class Subclass
    {
        public string id;

        public string team;

        public string name;

        public int need_rank;

        public string image;

        public Subclass() { }

        public Subclass(string id, string team, string name, int need_rank, string image)
        {
            this.id = id;
            this.team = team;
            this.name = name;
            this.need_rank = need_rank;
            this.image = image;
        }
    }
}
