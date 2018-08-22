using (WebClient wc = new WebClient()) {
    string URI = "https://starbucksthcampaign.com/c/quiz_2018_summer_3";
	string Phone = "0812345678"
	string myParameters = string.Format("mobilephone_1={0}&mobilephone_2={0}&mobilephone_3={0}&mobilephone_4={0}&question_89=318&question_99=358&question_85=302", Phone);
	
	    wc.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
    wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    wc.Headers.Add("Referer", "https://starbucksthcampaign.com/c/quiz_2018_summer_3");
    wc.Headers.Add("Origin", "https://starbucksthcampaign.com");
    wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36");
	
	wc.UploadString(URI, myParameters));
}

