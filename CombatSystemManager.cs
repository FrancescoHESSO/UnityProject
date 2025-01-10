using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;


    public class CombatSystemManager : MonoBehaviour
    {
        public CharacterMovement playerCharacter; // Riferimento al giocatore
        public ComputerMovement computerCharacter; // Riferimento al computer

        public TextMeshProUGUI combatLogText; // Testo per visualizzare i log di combattimento
        public TextMeshProUGUI combatStatusText; // Testo per visualizzare lo stato del combattimento (es. "Combat Started!")

        private bool combatActive = false;

        private void OnCollisionEnter(Collision collision)
        {
            // Verifica che i due personaggi siano entrati in collisione
            if ((collision.gameObject == playerCharacter.gameObject || collision.gameObject == computerCharacter.gameObject) && !combatActive)
            {
                StartCombat();
            }
        }

        public void StartCombat()
        {
            combatActive = true;

            // Disabilita il movimento di entrambi i personaggi
            playerCharacter.DisableMovement();
            computerCharacter.DisableMovement();

            // Mostra il messaggio "Combat Started!"
            combatStatusText.text = "Combat Started!";
            Debug.Log("Combat Started!");

            // Avvia il ciclo di combattimento
            StartCoroutine(CombatLoop());
        }

        private IEnumerator CombatLoop()
        {
            while (playerCharacter.Life > 0 && computerCharacter.Life > 0)
            {
                // Turno del giocatore
                yield return PlayerTurn();

                // Verifica se il computer è morto
                if (computerCharacter.Life <= 0)
                {
                    EndCombat("Player Wins!");
                    yield break;
                }

                // Turno del computer
                yield return ComputerTurn();

                // Verifica se il giocatore è morto
                if (playerCharacter.Life <= 0)
                {
                    EndCombat("Computer Wins!");
                    yield break;
                }

                // Attendi un breve intervallo tra i turni
                yield return new WaitForSeconds(1);
            }
        }

        private IEnumerator PlayerTurn()
        {
            int playerRoll = Random.Range(1, 7); // Tira un dado da 1 a 6
            if (playerRoll > computerCharacter.Defense)
            {
                computerCharacter.Life--;
                combatLogText.text = $"Player rolls {playerRoll} - Success! Computer Life: {computerCharacter.Life}";
                Debug.Log($"Player rolls {playerRoll} - Success! Computer Life: {computerCharacter.Life}");
            }
            else
            {
                combatLogText.text = $"Player rolls {playerRoll} - Missed!";
                Debug.Log($"Player rolls {playerRoll} - Missed!");
            }

            // Attendi un breve intervallo per mostrare il risultato
            yield return new WaitForSeconds(1);
        }

        private IEnumerator ComputerTurn()
        {
            int computerRoll = Random.Range(1, 7); // Tira un dado da 1 a 6
            if (computerRoll > playerCharacter.Defense)
            {
                playerCharacter.Life--;
                combatLogText.text = $"Computer rolls {computerRoll} - Success! Player Life: {playerCharacter.Life}";
                Debug.Log($"Computer rolls {computerRoll} - Success! Player Life: {playerCharacter.Life}");
            }
            else
            {
                combatLogText.text = $"Computer rolls {computerRoll} - Missed!";
                Debug.Log($"Computer rolls {computerRoll} - Missed!");
            }

            // Attendi un breve intervallo per mostrare il risultato
            yield return new WaitForSeconds(1);
        }

        private void EndCombat(string result)
        {
            combatActive = false;
            combatStatusText.text = result;
            Debug.Log(result);
        }
    }
