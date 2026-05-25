public static int midSRandom()
{ Random rnd = new Random(); midSX = rnd.Next(1, 51); return midSX; }
public static int botSRandom()
{ Random rnd = new Random(); botSX = rnd.Next(1, 51); return botSX; }
public static int topSRandom()
{ Random rnd = new Random(); topSX = rnd.Next(1, 51); return topSX; }

public static int leftSRandom()
{ Random rnd = new Random(); leftSX = rnd.Next(1, 51); return leftSX; }
public static int rightSRandom()
{ Random rnd = new Random(); rightSX = rnd.Next(1, 51); return rightSX; }
public static int BallButtonRandom()
{ Random rnd = new Random(); ballRR = rnd.Next(1, 51); return ballRR; }


// Done
// team: "A" = away, "H" = home
public static string generateHitType(team)
{
    Random rnd = new Random();
	// 0 - 99 (100 values)
    int x = rnd.Next(0, 100);
	// 64%
    if (x <= 63)
    {
		if (team == "A") {
			awaySingleOccurred();
		}
        else {
			homeSingleOccurred();
		}
        return "single.";
    }
	// 20%
    else if (x <= 83)
    {
        if (team == "A") {
			awayDoubleOccurred();
		}
        else {
			homeDoubleOccurred();
		}
        return "double.";
	// 2%
    }
    else if (x <= 85)
    {
        if (team == "A") {
			awayTripleOccurred();
		}
        else {
			homeTripleOccurred();
		}
        return "triple.";
    }
	// 14%
    else
    {
        if (team == "A") {
			awayHROccurred();
		}
        else {
			homeHROccurred();
		}
        return "homerun.";
    }
}

public static void SingleOccurred(team)
{
    Random rnd = new Random();
	// 0 - 99 (100 values)
    int outcome = rnd.Next(0, 100);

    if (scenario_NoOneOn()) //done
    {
        Green_FirstBase(); Black_SecondBase(); Black_ThirdBase();
    }
    else if (scenario_OnFirst()) //done
    {
        //55%
		if (outcome <= 54) {
            //first and second
			Green_FirstBase(); Green_SecondBase(); Black_ThirdBase(); 
		}
		//45%
		else
		{
            //first and third
			Green_FirstBase(); Black_SecondBase(); Green_ThirdBase();
		}
    }
    else if (scenario_OnSecond()) //done
    {
        //55% runner scores
		if (outcome <= 54) {
			Green_FirstBase(); Black_SecondBase(); Black_ThirdBase(); 
            if (team == 'A')
			{
				atScore++;
				ATScoreTX.Text = atScore.ToString();
			}
			else 
			{
				htScore++;
				HTScoreTX.Text = htScore.ToString();
			}
		}
		//45%
		else
		{
            //first and third
			Green_FirstBase(); Black_SecondBase(); Green_ThirdBase();
		}
        //runner thrown out??
    }
    else if (scenario_OnThird()) //done
    {
        //runner from third scores
        Green_FirstBase(); Black_SecondBase(); Black_ThirdBase();
        if (team == 'A')
        {
            atScore++;
            ATScoreTX.Text = atScore.ToString();
        }
        else 
        {
            htScore++;
            HTScoreTX.Text = htScore.ToString();
        }
    }
    else if (scenario_OnFirstSecond()) //4
    {
        Green_FirstBase(); Green_SecondBase(); Green_ThirdBase(); //bases loaded or runner would score 1st/2nd left
        ATScoreTX.Text = atScore.ToString();
    }
    else if (scenario_OnFirstThird()) //5
    {
        Green_FirstBase(); Green_SecondBase(); Black_ThirdBase(); atScore++;
        ATScoreTX.Text = atScore.ToString();
    }
    else if (scenario_OnSecondThird()) //6
    {
        Green_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore++; //2 runs...
        ATScoreTX.Text = atScore.ToString();
    }
    else if (scenario_BasesLoaded()) //7
    {
        Green_FirstBase(); Green_SecondBase(); Green_ThirdBase(); atScore++; //2 runs...
        ATScoreTX.Text = atScore.ToString();
    }
}

//IP
public static void DoubleOccurred(team)
{
	Random rnd = new Random();
	// 0 - 99 (100 values)
    int outcome = rnd.Next(0, 100);
	
    if (scenario_NoOneOn()) //0
    {
        Black_FirstBase(); Green_SecondBase(); Black_ThirdBase();
        ATScoreTX.Text = atScore.ToString();
    }
    else if (scenario_OnFirst()) //done
    {
		//60%
		if (outcome <= 59) {
			//runner from first scores
			Black_FirstBase(); Green_SecondBase(); Black_ThirdBase(); 
			if (team == 'A')
			{
				atScore++;
				ATScoreTX.Text = atScore.ToString();
			}
			else 
			{
				htScore++;
				HTScoreTX.Text = htScore.ToString();
			}
		}
		//40%
		else
		{
			//runner at first only gets to third
			Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); 
		}
    }
    else if (scenario_OnSecond()) //done
    {
        Black_FirstBase(); Green_SecondBase(); Black_ThirdBase();
		if (team == 'A')
		{
			atScore++;
			ATScoreTX.Text = atScore.ToString();
		}
		else 
		{
			htScore++;
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnThird()) //done
    {
        Black_FirstBase(); Green_SecondBase(); Black_ThirdBase();
		if (team == 'A')
		{
			atScore++;
			ATScoreTX.Text = atScore.ToString();
		}
		else 
		{
			htScore++;
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnFirstSecond()) //4
    {
        Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); atScore++; //2 runs?
        ATScoreTX.Text = atScore.ToString();
    }
    else if (scenario_OnFirstThird()) //5
    {
        Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); atScore++; //2 runs?
        ATScoreTX.Text = atScore.ToString();
    }
    else if (scenario_OnSecondThird()) //done, 2 runs score auto
    {
        Black_FirstBase(); Green_SecondBase(); Black_ThirdBase();
        if (team == 'A')
		{
			atScore += 2;
			ATScoreTX.Text = atScore.ToString();
		}
		else 
		{
			htScore += 2;
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_BasesLoaded()) //7
    {
        Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); atScore += 2; //3 runs??
        ATScoreTX.Text = atScore.ToString();
    }
}

// Once we change these fxs to A/H neutral, it will be done
public static void TripleOccurred(team)
{	
    if (scenario_NoOneOn()) //0
    {
        Black_FirstBase(); Black_SecondBase(); Green_ThirdBase();
    }
    else if (scenario_OnFirst()) //1
    {
        Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore++;
        ATScoreTX.Text = atScore.ToString();
    }
    else if (scenario_OnSecond()) //2
    {
        Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore++;
        ATScoreTX.Text = atScore.ToString();
    }
    else if (scenario_OnThird()) //3
    {
        Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore++;
        ATScoreTX.Text = atScore.ToString();
    }
    else if (scenario_OnFirstSecond()) //4
    {
        Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore += 2;
        ATScoreTX.Text = atScore.ToString();
    }
    else if (scenario_OnFirstThird()) //5
    {
        Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore += 2;
        ATScoreTX.Text = atScore.ToString();
    }
    else if (scenario_OnSecondThird()) //6
    {
        Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore += 2;
        ATScoreTX.Text = atScore.ToString();
    }
    else if (scenario_BasesLoaded()) //7
    {
        Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); atScore += 3;
        ATScoreTX.Text = atScore.ToString();
    }
}

// Done
public static void HROccurred(team)
{
    if (scenario_NoOneOn()) //0
    {
        if (team == "A") {
			atScore++;
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			htScore++;
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnFirst()) //1
    {
        if (team == "A") {
			atScore += 2;
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			htScore += 2;
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnSecond()) //2
    {
        if (team == "A") {
			atScore += 2;
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			htScore += 2;
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnThird()) //3
    {
        if (team == "A") {
			atScore += 2;
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			htScore += 2;
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnFirstSecond()) //4
    {
        if (team == "A") {
			atScore += 3;
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			htScore += 3;
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnFirstThird()) //5
    {
        if (team == "A") {
			atScore += 3;
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			htScore += 3;
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnSecondThird()) //6
    {
        if (team == "A") {
			atScore += 3;
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			htScore += 3;
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_BasesLoaded()) //7
    {
        if (team == "A") {
			atScore += 4;
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			htScore += 4;
			HTScoreTX.Text = htScore.ToString();
		}
    }
    clearBases();
}

public static void GroundOut(team)
{
	Random rnd = new Random();
	// 0 - 99 (100 values) ---- error / only one out chance
    int outcome = rnd.Next(0, 100);
	
    if (scenario_OnFirst())
    {
		// Double play
        Black_FirstBase(); Black_SecondBase(); Black_ThirdBase(); 
		outCount += 2;
    }
	else if (scenario_OnSecond())
    {
        //48%
		if (outcome <= 47)
		{
			//ground ball to right side 2nd base runner to 3rd base
			Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); 
			// 1 more out
			outCount++;
		}
		//48%
		else if (outcome <= 95)
		{
			//ground ball to left side, 2nd base runner stays
			Black_FirstBase(); Green_SecondBase(); Black_ThirdBase(); 
			// 1 more out
			outCount++;
		}
		//4%
		else {
			//ground ball to left side, 2nd base runner out at 3rd
			Green_FirstBase(); Black_SecondBase(); Black_ThirdBase(); 
			// 1 more out
			outCount++;
		}
    }
	else if (scenario_OnThird())
    {
		//98%
		if (outcome <= 97)
		{
			//clear bases on a ground ball out with runner on 3rd
			Black_FirstBase(); Black_SecondBase(); Black_ThirdBase(); 
			// 1 more out
			outCount++;
			//increment score
			if (team == "A") {
				atScore++;
				ATScoreTX.Text = atScore.ToString();
			}
			else {
				htScore++;
				HTScoreTX.Text = htScore.ToString();
			}
		}
		//2% - runner does not score
		else {
			Green_FirstBase(); Black_SecondBase(); Black_ThirdBase(); 
			// 1 more out
			outCount++;
		}
    }
    else if (scenario_OnFirstSecond())
    {
		// Double play
        Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); 
		outCount += 2;
    }
    else if (scenario_OnFirstThird())
    {
		// Double play
        Black_FirstBase(); Black_SecondBase(); Green_ThirdBase(); 
		outCount += 2;
    }
    else if (scenario_BasesLoaded())
    {
		// Double play
        Black_FirstBase(); Green_SecondBase(); Green_ThirdBase(); 
		outCount += 2;
    }
    else
    {
        outCount++;
    }
}

// Done
public static void Walk(team)
{
    if (scenario_NoOneOn()) //0
    {
        Green_FirstBase(); Black_SecondBase(); Black_ThirdBase();
		if (team == "A") {
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnFirst()) //1
    {
        Green_FirstBase(); Green_SecondBase(); Black_ThirdBase();
        if (team == "A") {
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnSecond()) //2
    {
        Green_FirstBase(); Green_SecondBase(); Black_ThirdBase();
        if (team == "A") {
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnThird()) //3
    {
        Green_FirstBase(); Black_SecondBase(); Green_ThirdBase();
        if (team == "A") {
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnFirstSecond()) //4
    {
        Green_FirstBase(); Green_SecondBase(); Green_ThirdBase();
        if (team == "A") {
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnFirstThird()) //5
    {
        Green_FirstBase(); Green_SecondBase(); Green_ThirdBase();
        if (team == "A") {
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_OnSecondThird()) //6
    {
        Green_FirstBase(); Green_SecondBase(); Green_ThirdBase();
        if (team == "A") {
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			HTScoreTX.Text = htScore.ToString();
		}
    }
    else if (scenario_BasesLoaded()) //7
    {
        if (team == "A") {
			atScore++;
			ATScoreTX.Text = atScore.ToString();
		}
        else {
			htScore++;
			HTScoreTX.Text = htScore.ToString();
		}
    }
}