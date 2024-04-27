using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Color tabIdle, tabHover, tabSelected;
    public TabButton selectedTab;

    private void Start()
    {
        ResetTabs();
    }

    public void Subscribe(TabButton tab)
    {
        if (tabButtons == null)
            tabButtons = new List<TabButton>();

        tabButtons.Add(tab);
    }

    public void OnTabEnter(TabButton tab)
    {
        if (selectedTab == null || tab != selectedTab)
            tab.background.color = tabHover;
    }

    public void OnTabExit(TabButton tab)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton tab)
    {
        if (selectedTab != null)
            selectedTab.Deselect();

        selectedTab = tab;
        selectedTab.Select();
        ResetTabs();
    }

    public void ResetTabs()
    {
        foreach(TabButton tab in tabButtons)
        {
            if (selectedTab != null && tab == selectedTab)
                tab.background.color = tabSelected;
            else
                tab.background.color = tabIdle;
        }
    }

}
