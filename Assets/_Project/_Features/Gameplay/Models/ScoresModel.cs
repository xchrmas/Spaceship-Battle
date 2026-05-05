using System;
using System.Collections.Generic;
using System.Linq;
using SpaceshipBattle.Services;

namespace SpaceshipBattle.Models
{
    [Serializable]
    public class ScoreItem
    {
        public int Score;
        public string Date;
    }

    [Serializable]
    public class Scoreboard
    {
        public ScoreItem[] Items;
    }

    public class ScoresModel
    {
        const string _storageKey = "Scoreboard";

        readonly IStorageService _storageService;

        public ScoresModel(IStorageService storageService)
        {
            _storageService = storageService;
            Scoreboard = new List<ScoreItem>();
        }

        public List<ScoreItem> Scoreboard { get; }

        public void Add(ScoreItem item)
        {
            var list = Scoreboard.ToList();
            list.Add(item);

            var ordered = list
                .OrderByDescending(x => x.Score)
                .Take(10)
                .ToList();

            UpdateScoreboard(ordered);
        }

        public void Save()
        {
                _storageService.Save(_storageKey,
                new Scoreboard { Items = Scoreboard.ToArray() });
        }

        public void Load()
        {
            var scoreboard = _storageService.Load<Scoreboard>(_storageKey);
            if (scoreboard?.Items != null)
                UpdateScoreboard(scoreboard.Items);
        }

        private void UpdateScoreboard(IList<ScoreItem> items)
        {
            if (items == null) return;
            Scoreboard.Clear();
            foreach (var item in items)
                Scoreboard.Add(item);
        }
    }
}