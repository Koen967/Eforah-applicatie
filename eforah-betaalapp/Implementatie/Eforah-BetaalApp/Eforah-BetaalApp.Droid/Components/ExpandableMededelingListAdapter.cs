using System;
using Android.Widget;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android;
using Eforah_BetaalApp.Implementation.Models;
using Android.Graphics;
using System.Net;
using Eforah_BetaalApp.Droid.Controllers;

namespace Eforah_BetaalApp.Droid.Components
{
    public class ExpandableMededelingListAdapter : BaseExpandableListAdapter
    {
        private Activity _context;
        private List<string> _listDataHeader;
        private Dictionary<string, List<string>> _listDataChild;
        private List<MededelingModel> _listMededeling;
        private string datetimeformat = "HH:mm d\\/M\\/yyyy";

        public ExpandableMededelingListAdapter(Activity context, List<MededelingModel> listmededeling)
        {
            _context = context;

            _listMededeling = listmededeling;
            _listDataHeader = new List<string>();
            _listDataChild = new Dictionary<string, List<string>>();

            foreach (MededelingModel m in listmededeling)
            {
                _listDataHeader.Add(m.titel);
                _listDataChild.Add(m.titel, null);
            }

        }

        #region Childeren
        /// <summary>
        /// Haal de child op. N.V.T. in deze adapter.
        /// </summary>
        /// <param name="groupPosition">Positie van groep</param>
        /// <param name="childPosition">positie van child</param>
        /// <returns>Return altijd de ene child van de groep</returns>
        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            if (groupPosition < 0)
            {
                return _listDataChild[_listDataHeader[0]][0];
            }
            else if (groupPosition >= _listDataHeader.Count)
            {
                return _listDataChild[_listDataHeader[GroupCount - 1]][0];
            }
            return _listDataChild[_listDataHeader[groupPosition]][0];
        }

        /// <summary>
        /// Get id van child
        /// </summary>
        /// <param name="groupPosition">Positie van groep</param>
        /// <param name="childPosition">positie van child</param>
        /// <returns>Return altijd 0, want er is slechts één child</returns>
        public override long GetChildId(int groupPosition, int childPosition)
        {
            return 0;
            //return childPosition;
        }

        /// <summary>
        /// Maak de view van een child.
        /// Voor deze adapter is er telkens maar één child en dat zijn de gegevens van een mededeling.
        /// </summary>
        /// <param name="groupPosition">The positie van de mededeling</param>
        /// <param name="childPosition">N.V.T.</param>
        /// <param name="isLastChild">N.V.T. altijd waar</param>
        /// <param name="convertView">View om aan te passen</param>
        /// <param name="parent">N.V.T.</param>
        /// <returns>De view van de child</returns>
        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            MededelingModel child;
            if (groupPosition < 0)
                child = _listMededeling[0];
            else if (groupPosition >= _listMededeling.Count)
                child = _listMededeling[_listMededeling.Count - 1];
            else
                child = _listMededeling[groupPosition];

            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.MededelingListRowChild, null);
            }

            // Get views
            TextView mededeling = (TextView)convertView.FindViewById(Resource.Id.mededeling_mededeling);
            TextView plaatsingDatum = (TextView)convertView.FindViewById(Resource.Id.mededeling_plaatsingDatum);

            mededeling.Text = "Straat: " + child.mededeling;
            plaatsingDatum.Text = "Adres: " + MededelingActivity.convertDateTime(child.plaatsingDatum, datetimeformat);

            return convertView;
        }

        /// <summary>
        /// Hoeveel childeren te groep heeft.
        /// Voor deze is dat altijd 0.
        /// </summary>
        /// <param name="groupPosition">De positie van de groep</param>
        /// <returns>Hoeveel childeren er in de groep zijn.</returns>
        public override int GetChildrenCount(int groupPosition)
        {
            return 1;
            //return _listDataChild[_listDataHeader[groupPosition]].Count;
        }
        #endregion

        #region Group
        /// <summary>
        /// Krijg groep titel van een bepaalde groep
        /// </summary>
        /// <param name="groupPosition">De Positie van de groep</param>
        /// <returns>De titel van de groep</returns>
        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            if (groupPosition < 0)
            {
                return _listDataHeader[0];
            }
            else if (groupPosition >= _listDataHeader.Count)
            {
                return _listDataHeader[GroupCount - 1];
            }
            return _listDataHeader[groupPosition];
        }

        /// <summary>
        /// Totaal van alle groepen
        /// </summary>
        public override int GroupCount
        {
            get
            {
                return _listDataHeader.Count;
            }
        }

        /// <summary>
        /// Get id van groep
        /// </summary>
        /// <param name="groupPosition">positie van groep</param>
        /// <returns>groep id. Dit is positie.</returns>
        public override long GetGroupId(int groupPosition)
        {
            if (groupPosition < 0)
            {
                return 0;
            }
            else if (groupPosition >= _listDataHeader.Count)
            {
                return GroupCount - 1;
            }
            return groupPosition;
        }

        /// <summary>
        /// Get de groep view.
        /// Hier worden de titels (titels van mededelingen) toegevoegd.
        /// </summary>
        /// <param name="groupPosition">De positie van de groep</param>
        /// <param name="isExpanded">Altijd ja en dus genegeerd.</param>
        /// <param name="convertView">De view om aan te passen.</param>
        /// <param name="parent">N.V.T.</param>
        /// <returns>De Aangepaste view</returns>
        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            string headerTitle = (string)GetGroup(groupPosition);

            convertView = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.MededelingListRow, null);
            var mededeling = (TextView)convertView.FindViewById(Resource.Id.mededeling);
            mededeling.Text = headerTitle;

            return convertView;
        }
        #endregion

        public override bool HasStableIds
        {
            get
            {
                return true;
            }
        }
        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return false;
        }

        class ViewHolderItem : Java.Lang.Object
        {
        }
    }
}