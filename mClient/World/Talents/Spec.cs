using mClient.Constants;
using mClient.DBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mClient.World.Talents
{
    /// <summary>
    /// Holds data that represents how a bot should spec their talents. User created.
    /// </summary>
    public class Spec
    {
        #region Declarations

        public const int MAX_TALENT_POINTS = 51;

        #endregion

        #region Constructors

        public Spec()
        {
            Id = (int)SpecManager.Instance.GetNextId();
            Talents = new uint[MAX_TALENT_POINTS + 1];
        }

        public Spec(string name) : this()
        {
            Name = name;
        }

        #endregion

        #region Properties

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Classname ForClass { get; set; }

        public uint[] Talents { get; set; }

        /// <summary>
        /// Returns the talent spec currently being used based on talents data
        /// </summary>
        public MainSpec TalentSpec
        {
            get
            {
                // Get talent entry for each talent selected and tally up the total number of points in each talent tab. The tab with the highest points wins.
                var talentTabCounts = new Dictionary<uint, int>();
                foreach (var t in Talents)
                {
                    var talentEntry = TalentTable.Instance.getBySpell(t);
                    if (talentEntry != null)
                    {
                        var talentTab = TalentTabTable.Instance.getById(talentEntry.TalentTabId);
                        if (talentTabCounts.ContainsKey(talentTab.TabPage))
                            talentTabCounts[talentTab.TabPage]++;
                        else
                            talentTabCounts.Add(talentTab.TabPage, 1);
                    }
                }

                // get the KVP that has the highest value
                var maxValue = talentTabCounts.Select(kvp => kvp.Value).DefaultIfEmpty(0).Max();
                if (maxValue == 0)
                    return MainSpec.NONE;
                var value = talentTabCounts.Where(kvp => kvp.Value == maxValue).FirstOrDefault();
                var tab = value.Key;

                // Get the spec this tab relates to from the class logic
                return PlayerClassLogic.GetSpecFromTalentTab(ForClass, tab);
            }
        }

        #endregion
    }
}
