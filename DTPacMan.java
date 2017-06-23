package pacman.controllers;

import java.util.ArrayList;
import pacman.controllers.Controller;
import pacman.game.Game;
import pacman.game.internal.Node;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.util.LinkedList;
import java.util.List;
import java.util.Scanner;

import static pacman.game.Constants.*;


public class DTPacMan extends Controller<MOVE>{
	
	private static final int Edible_Ghost_Distance=30;		//minimum distance between ghost and pacman for Ms Pacman to execute a move on powerPill.
	private static final int Nonedible_Ghost_Distance=15;
	

	int[] readArray= new int[23];
	
	
	
	public MOVE getMove(Game game,long timeDue)
	{	
		
		
		int current=game.getPacmanCurrentNodeIndex();
		
		GHOST minEdibleGhost=null;	
		GHOST minNonEdibleGhost=null;	
		
		int minedibleDistance=100000;
		int minNonedibleDistance = 100000;
		int powerPillsAvail = 1;
				
		//calculate the target indices of pills
		int[] pills=game.getPillIndices();
		int[] powerPills=game.getPowerPillIndices();		
		
		ArrayList<Integer> targetPills=new ArrayList<Integer>();
		ArrayList<Integer> PowerTarget=new ArrayList<Integer>();

		
		for(int i=0;i<pills.length;i++)					//check which pills are available			
			if(game.isPillStillAvailable(i))
				targetPills.add(pills[i]);
		
		for(int i=0;i<powerPills.length;i++)			//check which power pills are available
			if(game.isPowerPillStillAvailable(i))
				PowerTarget.add(powerPills[i]);	
			else
				powerPillsAvail = 0;
		
		int[] targetPillsArray=new int[targetPills.size()];		//convert from target pills ArrayList to array
		int[] PowertargetArray=new int[PowerTarget.size()];		//convert from power target ArrayList to array

		for(int i=0;i<PowertargetArray.length;i++)
			PowertargetArray[i]=PowerTarget.get(i);
		
		for(int i=0;i<targetPillsArray.length;i++)
			targetPillsArray[i]=targetPills.get(i);
	
		
		for(GHOST ghost : GHOST.values())
		{					
			if(game.getGhostEdibleTime(ghost)>0)
			{
				int edibledistance = game.getShortestPathDistance(current,game.getGhostCurrentNodeIndex(ghost));

				if(edibledistance < minedibleDistance)
				{
					minedibleDistance = edibledistance;
					minEdibleGhost = ghost;
					
				}
			}
			else 
				if(game.getGhostEdibleTime(ghost)==0)
				{
					
					int nonedibledistance = game.getShortestPathDistance(current,game.getGhostCurrentNodeIndex(ghost));
					

					if(nonedibledistance < minNonedibleDistance && nonedibledistance>-1)
					{
						//System.out.println(nonedibledistance);
						minNonedibleDistance = nonedibledistance;
						minNonEdibleGhost = ghost;
					}
				}
		}
		
		BufferedReader br = null;
		int j = 0;
        try
        {      //  System.out.println("reads1");  	
        	br =new BufferedReader(new InputStreamReader(new FileInputStream("data/Assignment3/DTtextfile")));	 
        	// System.out.println("reads");
        	String input=br.readLine();
        	
            while(input!=null)
            {	
            	System.out.println(input);
               switch(input)
               {
               case "availableTime": readArray[j] = 0; break;
               case "ghostDistance":	readArray[j] = minNonedibleDistance; 	break;
               case "minSafeDistance": 	readArray[j] = Nonedible_Ghost_Distance; 	break;
               case "minAttackDistance":	readArray[j] = minedibleDistance;	break;
               case "edibleTime":	if(minEdibleGhost != null) 
            	   						{
            	   							readArray[j] = game.getGhostEdibleTime(minEdibleGhost);	
            	   						}
               							else
               							readArray[j] = 0;
               						break;
               case "minTimeToEat":	readArray[j] = 1;	break;
               case "maxTimeToEat":	readArray[j] = 2; break;
               case "eatPowerPill":	readArray[j] = 9991; break;
               case "eatPill": readArray[j] = 9992;	break;
               case "runAway":	readArray[j] = 9993;	break;
               case "eatGhost":	readArray[j] = 9994;	break;
               case "powerPillAvail": readArray[j] = powerPillsAvail;	break;
               case "yes": readArray[j] = 1;	break;
               case "no": readArray[j]	= 0;	break;
               }
               System.out.println(j+ " " + readArray[j]);
               j++;
               input=br.readLine();
            }
           
        }
        catch(IOException ioe)
        {
        	 System.out.println("nothing");
        }
		
		
		//Starting the decision tree
		if (readArray[0]>readArray[1])
		{
			if(readArray[2] < readArray[3])
			{
				if(readArray[6] < readArray[7])
				{	
					if(readArray[13]>readArray[14])
					{
						//eat ghost
						switch(readArray[20])
						{
						case 9991:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,PowertargetArray,DM.PATH),DM.PATH);
						
						case 9992:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);

						case 9993:	return game.getNextMoveAwayFromTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minNonEdibleGhost),DM.PATH);

						case 9994:	return game.getNextMoveTowardsTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minEdibleGhost),DM.PATH);
						
						default: return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);					

						}
				
					}
					else
						{
						//run
						switch(readArray[21])
						{
						case 9991:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,PowertargetArray,DM.PATH),DM.PATH);
						
						case 9992:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);

						case 9993:	return game.getNextMoveAwayFromTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minNonEdibleGhost),DM.PATH);

						case 9994:	return game.getNextMoveTowardsTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minEdibleGhost),DM.PATH);
						
						default: return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);					

						}
						}

				}
				else
					{//run
					switch(readArray[15])
					{
					case 9991:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,PowertargetArray,DM.PATH),DM.PATH);
					
					case 9992:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);

					case 9993:	return game.getNextMoveAwayFromTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minNonEdibleGhost),DM.PATH);

					case 9994:	return game.getNextMoveTowardsTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minEdibleGhost),DM.PATH);
					
					default: return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);					

					}
					}
			}
			else
				if(readArray[8]>readArray[9])
				{
					//eat ghost

	                
	             //   System.out.println(readArray[15]);
	                
					switch(readArray[16])
					{
					case 9991:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,PowertargetArray,DM.PATH),DM.PATH);
					
					case 9992:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);

					case 9993:	return game.getNextMoveAwayFromTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minNonEdibleGhost),DM.PATH);

					case 9994:	return game.getNextMoveTowardsTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minEdibleGhost),DM.PATH);
					
					default: return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);					

					}				
				}
				else
					{
					//run
	               // System.out.println(readArray[16]);

					switch(readArray[17])
					{
					case 9991:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,PowertargetArray,DM.PATH),DM.PATH);
					
					case 9992:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);

					case 9993:	return game.getNextMoveAwayFromTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minNonEdibleGhost),DM.PATH);

					case 9994:	return game.getNextMoveTowardsTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minEdibleGhost),DM.PATH);
					
					default: return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);					

					}
					}
		}
		else
			if(readArray[4] < readArray[5])
			{
				//powerpill
              //  System.out.println(readArray[10]);
				if(readArray[10]==readArray[11])
				{	switch(readArray[18])
					{
					case 9991:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,PowertargetArray,DM.PATH),DM.PATH);
					
					case 9992:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);
	
					case 9993:	return game.getNextMoveAwayFromTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minNonEdibleGhost),DM.PATH);
	
					case 9994:	return game.getNextMoveTowardsTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minEdibleGhost),DM.PATH);
	
					default: return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);					
					}
				}
				else
					switch(readArray[19])
					{
					case 9991:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,PowertargetArray,DM.PATH),DM.PATH);
					
					case 9992:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);
	
					case 9993:	return game.getNextMoveAwayFromTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minNonEdibleGhost),DM.PATH);
	
					case 9994:	return game.getNextMoveTowardsTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minEdibleGhost),DM.PATH);
	
					default: return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);					
					}
					
			}
			else
				{//pill
              //  System.out.println(readArray[11]);

					switch(readArray[12])
					{
					case 9991:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,PowertargetArray,DM.PATH),DM.PATH);
					
					case 9992:	return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);
	
					case 9993:	return game.getNextMoveAwayFromTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minNonEdibleGhost),DM.PATH);
	
					case 9994:	return game.getNextMoveTowardsTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minEdibleGhost),DM.PATH);
					
					default: return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);					
					}
				}
		
		
		//hard coded decision tree
		/*
		if (minEdibleGhost != null)
		{
			if(minNonedibleDistance < Nonedible_Ghost_Distance)
			{
				if(minedibleDistance < minNonedibleDistance)
				{	
					if(game.getGhostEdibleTime(minEdibleGhost)>2)
					{
						//eat ghost
						return game.getNextMoveTowardsTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minEdibleGhost),DM.PATH);

					}
					else
						{
						//run
						return game.getNextMoveAwayFromTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minNonEdibleGhost),DM.PATH);
						}

				}
				else
					{//run
					return game.getNextMoveAwayFromTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minNonEdibleGhost),DM.PATH);
					}
			}
			else
				if(game.getGhostEdibleTime(minEdibleGhost)>1)
				{
					//eat ghost
					return game.getNextMoveTowardsTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minEdibleGhost),DM.PATH);
				}
				else
					{
					//run
					return game.getNextMoveAwayFromTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minNonEdibleGhost),DM.PATH);
					}
		}
		else
			if(minNonedibleDistance < Nonedible_Ghost_Distance)
			{
				//powerpill
				 if(powerPillsAvail == 1)
					return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,PowertargetArray,DM.PATH),DM.PATH);
				else
					return game.getNextMoveAwayFromTarget(game.getPacmanCurrentNodeIndex(),game.getGhostCurrentNodeIndex(minNonEdibleGhost),DM.PATH);

			}
			else
				{//pill
				return game.getNextMoveTowardsTarget(current,game.getClosestNodeIndexFromNodeIndex(current,targetPillsArray,DM.PATH),DM.PATH);
				}
		*/
	}
}

		
