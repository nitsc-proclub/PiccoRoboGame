using System.Collections;
using System.Linq;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    DevGameManager gameManager;

    [SerializeField]
    GameObject baseA;

    [SerializeField]
    GameObject baseB;
    
    [SerializeField]
    Transform spawnPointsA;

    [SerializeField]
    Transform spawnPointsB;

    HealthBarFactory healthBarFactory;

    void Awake()
    {
        healthBarFactory = GetComponent<HealthBarFactory>();
        healthBarFactory.AssignToCharacter(baseA.GetComponent<Character>(), 60);
        healthBarFactory.AssignToCharacter(baseB.GetComponent<Character>(), 60);

        var myTeam = gameManager.GetAssignedTeam();
        if (myTeam == Team.TeamA || myTeam == Team.TeamB)
        {
            var idx = gameManager.GetAllAssignedTeam()
                .OrderBy(v => v.Item1.ActorNumber)
                .Where(v => v.Item2 == myTeam)
                .Select((v, i) => (actornum: v.Item1.ActorNumber, index: i))
                .Where(v => v.actornum == PhotonNetwork.LocalPlayer.ActorNumber)
                .First()
                .index;
            Transform spawnPoints = myTeam switch
            {
                Team.TeamA => spawnPointsA,
                Team.TeamB => spawnPointsB,
                _ => null
            };
            var spawnPos = spawnPoints.GetChild(idx).position;
            var player = PhotonNetwork.Instantiate(
                "DevPlayer",
                spawnPos,
                Quaternion.identity);
            player.GetComponent<ScriptMachine>().enabled = true;

            var character = player.GetComponent<Character>();
            character.Team = myTeam;
            character.Display = DisplayKind.OwnerName;

            healthBarFactory.AssignToCharacter(character, 50);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
