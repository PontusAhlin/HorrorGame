using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;

using UnityEditor;
using UnityEngine.Rendering;

namespace UnityEngine.AdaptivePerformance.Tests
{
    [TestFixture(0, -1)] // No loaders, should never have any results
    [TestFixture(1, -1)] // 1 loader, fails so no active loaders
    [TestFixture(1, 0)] // All others, make sure the active loader is expected loader.
    [TestFixture(2, 0)]
    [TestFixture(2, 1)]
    [TestFixture(3, 2)]
    class ManualLifetimeTests
    {
        AdaptivePerformanceManagerSettings m_Manager;
        List<AdaptivePerformanceLoader> m_Loaders = new List<AdaptivePerformanceLoader>();
        int m_LoaderCount;
        int m_LoaderIndexToWin;

        public ManualLifetimeTests(int loaderCount, int loaderIndexToWin)
        {
            m_LoaderCount = loaderCount;
            m_LoaderIndexToWin = loaderIndexToWin;
        }

        [SetUp]
        public void SetupAdaptivePerformanceManagerTest()
        {
            m_Manager = ScriptableObject.CreateInstance<AdaptivePerformanceManagerSettings>();
            m_Manager.automaticLoading = false;

            m_Loaders = new List<AdaptivePerformanceLoader>();

            for (int i = 0; i < m_LoaderCount; i++)
            {
                DummyLoader dl = ScriptableObject.CreateInstance(typeof(DummyLoader)) as DummyLoader;
                dl.id = i;
                dl.shouldFail = (i != m_LoaderIndexToWin);
                m_Loaders.Add(dl);
                m_Manager.loaders.Add(dl);
            }
        }

        [TearDown]
        public void TeardownAdaptivePerformanceManagerTest()
        {
            Object.Destroy(m_Manager);
            m_Manager = null;
        }

        [UnityTest]
        public IEnumerator CheckActivatedLoader()
        {
            Assert.IsNotNull(m_Manager);

            yield return m_Manager.InitializeLoader();

            if (m_LoaderIndexToWin < 0 || m_LoaderIndexToWin >= m_Loaders.Count)
            {
                Assert.IsNull(m_Manager.activeLoader);
            }
            else
            {
                Assert.IsNotNull(m_Manager.activeLoader);
                Assert.AreEqual(m_Loaders[m_LoaderIndexToWin], m_Manager.activeLoader);
            }

            m_Manager.DeinitializeLoader();

            Assert.IsNull(m_Manager.activeLoader);

            m_Manager.loaders.Clear();
        }
    }
}
