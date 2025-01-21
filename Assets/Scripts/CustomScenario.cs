using System.Collections;
using UnityEngine.Perception.GroundTruth;
using UnityEngine.Perception.Randomization.Scenarios;
using Autohand;
using System;
using UnityEngine.Perception.Randomization.Samplers;

namespace UnityEngine.Perception.Randomization.Scenarios
{
    [AddComponentMenu("Perception/Scenarios/Custom Scenario")]
    public class CustomScenario : PerceptionScenario<CustomScenario.Constants>
    {
        public GameObject leftHand;
        public GameObject rightHand;
        Hand leftHandComponent;
        Hand rightHandComponent;

        public GameObject leftHandFollow;
        public GameObject rightHandFollow;
        private bool coroutineRunning = false;

        private Vector3 initialLeftHandFollowPosition;
        private Vector3 initialRightHandFollowPosition;

        private Quaternion initialLeftHandFollowRotation;
        private Quaternion initialRightHandFollowRotation;

        [Serializable]
        public class Constants : ScenarioConstants
        {
            [Tooltip("The index of the first iteration to execute.")]
            public int startIteration;

            [Tooltip("The number of iterations to run.")]
            public int iterationCount = 10;
        }

        protected override void OnStart()
        {
            base.OnStart();
            leftHandComponent = leftHand.GetComponent<Hand>();
            rightHandComponent = rightHand.GetComponent<Hand>();

            initialLeftHandFollowPosition = leftHandFollow.transform.position;
            initialRightHandFollowPosition = rightHandFollow.transform.position;

            initialLeftHandFollowRotation = leftHandFollow.transform.rotation;
            initialRightHandFollowRotation = rightHandFollow.transform.rotation;

        }
        protected override bool isScenarioComplete
        {
            get => currentIteration >= constants.startIteration + constants.iterationCount;
        }

        protected override void OnIterationStart()
        {
            base.OnIterationStart();

            //reset hand positions
            leftHandFollow.transform.position = initialLeftHandFollowPosition;
            rightHandFollow.transform.position = initialRightHandFollowPosition;

            //reset hand rotations
            leftHandFollow.transform.rotation = initialLeftHandFollowRotation;
            rightHandFollow.transform.rotation = initialRightHandFollowRotation;

            var uniformSampler = new UniformSampler(0f, 1f);

            float randomValue = uniformSampler.Sample();

            if (randomValue < 0.4f)
            {
                // 40% chance for left hand grab
                rightHandFollow.transform.position = new Vector3(initialRightHandFollowPosition.x + 0.1f, initialRightHandFollowPosition.y, initialRightHandFollowPosition.z - 0.05f);
                rightHandFollow.transform.rotation = RandomRotation(rightHandFollow.transform.rotation.eulerAngles.x, false);
                StartCoroutine(GrabReleaseCoroutine(leftHandComponent));
            }
            else if (randomValue < 0.8f)
            {
                // 40% chance for right hand grab
                leftHandFollow.transform.position = new Vector3(initialLeftHandFollowPosition.x - 0.1f, initialLeftHandFollowPosition.y, initialLeftHandFollowPosition.z - 0.05f);
                leftHandFollow.transform.rotation = RandomRotation(leftHandFollow.transform.rotation.eulerAngles.x, true);
                StartCoroutine(GrabReleaseCoroutine(rightHandComponent));
            }
            else
            {
                // 20% chance for both hands grab
                StartCoroutine(GrabReleaseCoroutine(leftHandComponent, rightHandComponent));
            }
        }

        IEnumerator GrabReleaseCoroutine(Hand hand)
        {
            coroutineRunning = true;
            yield return new WaitForSeconds(0.07f);
            hand.Grab();
            yield return new WaitUntil(() => hand.grabbing == false);

            yield return new WaitForSeconds(0.35f);
            hand.Release();

            yield return new WaitForSeconds(0.07f);
            coroutineRunning = false;
        }

        IEnumerator GrabReleaseCoroutine(Hand leftHand, Hand rightHand)
        {
            coroutineRunning = true;
            yield return new WaitForSeconds(0.07f);
            rightHand.Grab();
            leftHand.Grab();

            yield return new WaitUntil(() => leftHand.grabbing == false && rightHand.grabbing == false);

            yield return new WaitForSeconds(0.35f);
            rightHand.Release();
            leftHand.Release();

            yield return new WaitForSeconds(0.07f);
            coroutineRunning = false;
        }


        protected override bool isIterationComplete => !coroutineRunning;

        Quaternion RandomRotation(float originalXRotation, bool isLeftHand)
        {
            if (isLeftHand)
            {
                return Quaternion.Euler(originalXRotation, Random.Range(0f, 10f), Random.Range(0f, 90f));
            }

            return Quaternion.Euler(originalXRotation, Random.Range(0f, -10f), Random.Range(0f, 90));
        }

    }


}

