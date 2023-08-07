using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace Script.Quiz
{
    public class Bucket : MonoBehaviour, IDropHandler
    {
        public static Bucket Instance { set; get; }
        public Transform done;
        public Canvas canvas;
        
        [SerializeField] private Text description;
        [SerializeField] private Text descriptionTitle;
        [SerializeField] private Text[] bucketTitle;
        [SerializeField] private Transform animal;
        [SerializeField] private GameObject finishPanel;

        private const string CorrectAnswer = "CorrectAnswer";
        private const string IncorrectAnswer = "IncorrectAnswer";
        private bool _type;
        private int _number;
        private Transform _parent;
        private List<GameObject> _incorrectName = new();

        private void Awake()
        {
            Instance = this;
            BucketName();
            PlayerPrefs.DeleteAll();
            _parent = description.transform.parent;
        }

        private void BucketName()
        {
            _number = Random.Range(0, 4);
            switch (_number)
            {
                case 0:
                    bucketTitle[0].text = "Flying";
                    bucketTitle[1].text = "Non-Flying";
                    break;
                case 1:
                    bucketTitle[0].text = "Insect";
                    bucketTitle[1].text = "Non-Insect";
                    break;
                case 2:
                    bucketTitle[0].text = "Omnivorous";
                    bucketTitle[1].text = "Herbivorous";
                    break;
                case 3:
                    bucketTitle[0].text = "Lives in Group";
                    bucketTitle[1].text = "Lives in Solo";
                    break;
                case 4:
                    bucketTitle[0].text = "Lays Eggs";
                    bucketTitle[1].text = "Give Birth";
                    break;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var go = eventData.pointerDrag;
            var animalName = go.GetComponent<Animal>();
            description.text = animalName.desc;
            descriptionTitle.text = animalName.icon.name;
            go.transform.DOScale(Vector3.zero, .5f).OnComplete(() => go.SetActive(false));
            _parent.gameObject.SetActive(true);
            _parent.DOScale(Vector3.one, .5f);
            
            if ((_number == 0 && animalName.isFlying) || (_number == 1 && animalName.isInsect) ||
                (_number == 2 && animalName.isOmnivorous) || (_number == 3 && animalName.isLivesInGroup) ||
                (_number == 4 && animalName.isLaysEggs))
            {
                var correct = GetCorrectAnswerCount();
                correct++;
                SetCorrectAnswerCount(correct);
            }
            else
            {
                _incorrectName.Add(go);
                var incorrect = GetIncorrectAnswerCount();
                incorrect++;
                SetIncorrectAnswerCount(incorrect);
            }
        }

        public void CloseInfoPanel()
        {
            _parent.DOScale(Vector3.zero, .5f).OnComplete(()=>_parent.gameObject.SetActive(false));
            print(GetIncorrectAnswerCount() + GetCorrectAnswerCount());
            if (animal.childCount == GetIncorrectAnswerCount() + GetCorrectAnswerCount())
            {
                finishPanel.SetActive(true);
                finishPanel.transform.DOScale(Vector3.zero, .5f);
            }
        }

        #region ManagePlayerPref

        public void ShowIncorrectCard()
        {
            done.GetComponent<Image>().enabled = true;
            for (var i = 0; i < _incorrectName.Count - 1; i++)
                _incorrectName[i].SetActive(true);
        }

        private static int GetCorrectAnswerCount()
        {
            return PlayerPrefs.GetInt(CorrectAnswer);
        }

        private static void SetCorrectAnswerCount(int count) => PlayerPrefs.SetInt(CorrectAnswer, count);
        
        private static int GetIncorrectAnswerCount()
        {
            return PlayerPrefs.GetInt(IncorrectAnswer);
        }

        private static void SetIncorrectAnswerCount(int count) => PlayerPrefs.SetInt(IncorrectAnswer, count);
        
        #endregion
    }
}
