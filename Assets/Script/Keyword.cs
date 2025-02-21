using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyword
    {
        public string keyword;
        public string characterName;
        public string word;
        public string[] detail;
        public int detailIdx;

        public Keyword(string keyword, string details)
        {
            this.keyword = keyword;
            string[] tmp = keyword.Split(':');
            this.characterName = tmp[0];
            this.word = tmp[1];
            this.detail = details.Split('*');
            this.detailIdx = 0;
        }
    }
