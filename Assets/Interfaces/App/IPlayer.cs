using Domain;
using UnityEngine;

namespace App
{
    public interface IPlayer
    {
        string Name { get; set; }
        int Health { get; set; }
        int Score { get; }
        int Speed { get; set; }
        IController Controller { get; }
        GameObject GameObject { get; }
        IGun Gun { get; set; }
    }
}
