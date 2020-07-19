using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;
using System.Threading;

namespace pg_ludoleague
{
    public partial class ludoLeagueForm : Form
    {
        //The current config. A 2-player or 4-player game. //TODO
        int numOfPlayers = Configuration.currentPlayerCountConfig;

        //Sequence of jumping b/w colors while making moves.
        int[] moveSequence = new int[Configuration.currentPlayerCountConfig];

        //This is the path that every bead will take while moving on the board
        Path[][] pathVars;

        //The bead(s), 4 of each color
        Button[][] beadBtnVars;

        //The bead Class object for the beads
        Bead[][] beadVars;

        //The dice design
        Microsoft.VisualBasic.PowerPacks.OvalShape[] diceBeads = new Microsoft.VisualBasic.PowerPacks.OvalShape[9];

        //indicator for if its first ever move for any colored-bead (once per color)
        int[] fmove = new int[5];

        //Used for generating diceValues on each diceRoll
        Random diceValueRandomVar;
        int currentDiceValue = 0;

        int diceRoll = -1, dr = 1,osl = 0;

        //Used for maintaining number of open beads, 1 entry for each color
        int[] numbeadsopen;

        //UI element for the path Stepping stones
        RectangleShape[] movePath = new RectangleShape[53];

        RectangleShape[][] colorSpMovePath = new RectangleShape[4][];

        int[] map = new int[Configuration.currentPlayerCountConfig + 1];

        //used for setting path track
        int[] diffs = new int[] { 0,0,27,14,40};//DO Not Change these Values

        public ludoLeagueForm()
        {
            InitializeComponent();
            initializeGame();
        }

        /**
         * Method to initialize New Game
         * */
        private void initializeGame()
        {
            //initialize Path(s).
            for (int ival = 1; ival <= numOfPlayers; ival++)
            {
                map[ival] = new int();
                map[ival] = Configuration.playerColorConfigs[numOfPlayers][ival];

                //map[1] //First Color
                //map[2] //SecondColor
                //switch (map[1])
            }

            //path[][] pathVars;
            pathVars = new Path[numOfPlayers + 1][];

            //Button[][] Bead;
            beadBtnVars = new Button[numOfPlayers + 1][];

            //bead[][] beadVars;
            beadVars = new Bead[numOfPlayers + 1][];

            //int[] numbeadopen;
            numbeadsopen = new int[numOfPlayers + 1];

            colorSpMovePath = new RectangleShape[numOfPlayers + 1][];
            for (int ival = 1; ival <= (numOfPlayers); ival++)
            {
                colorSpMovePath[ival] = new RectangleShape[6]; //which Color = map[ivar];
                beadVars[ival] = new Bead[5];
                beadBtnVars[ival] = new Button[5];
                pathVars[ival] = new Path[57];
            }


            for (int ivar = 1; ivar <= numOfPlayers; ivar++)
            {
                for (int jvar = 1; jvar <= 56; jvar++)
                {
                    pathVars[ivar][jvar] = new Path();
                }
            }
            for (int ivar = 1; ivar <= numOfPlayers; ivar++)
            {
                diceBeads[ivar - 1] = new Microsoft.VisualBasic.PowerPacks.OvalShape();
                diceBeads[4 + ivar - 1] = new Microsoft.VisualBasic.PowerPacks.OvalShape();

                if (numOfPlayers == 2)
                {
                    if (ivar >= 3)
                    {
                        continue;
                    }
                }
                for (int jvar = 1; jvar <= 4; jvar++)
                {
                    beadBtnVars[ivar][jvar] = new Button();
                    beadBtnVars[ivar][jvar] = (Button)Controls["button" + ivar + "" + jvar];
                    beadVars[ivar][jvar] = new Bead();
                    colorSpMovePath[ivar][jvar] = new RectangleShape();
                }
                numbeadsopen[ivar] = new int();
                numbeadsopen[ivar] = 0;
                colorSpMovePath[ivar][5] = new RectangleShape();
                fmove[ivar] = 0;
            }
            diceBeads[8] = new Microsoft.VisualBasic.PowerPacks.OvalShape();
        }

        private void ludoLeagueForm_Load(object sender, EventArgs e)
        {
            formLoad();
        }

        /**
         * Method for tasks to do while on FormLoad();
         * */
        private void formLoad()
        {
            rectangleShapeDice.SendToBack();

            for (int mvar = 1; mvar <= 52; )
            {
                movePath[mvar] = new RectangleShape();
                int rs_index = shapeContainer1.Shapes.IndexOfKey("rectangleShape" + mvar);
                if (rs_index != -1)
                {
                    movePath[mvar] = (RectangleShape)shapeContainer1.Shapes.get_Item(rs_index);
                    if (!(movePath[mvar] == null))
                    {
                        if ((numOfPlayers == 2) || (numOfPlayers == 4))
                        {
                            pathVars[1][mvar].x = movePath[mvar].Left;// - 1
                            pathVars[1][mvar].y = movePath[mvar].Top; // - 1
                        }

                        for (int ivar = 2; ivar <= 4; ivar++)
                        {
                            if (ivar == 3 || ivar == 4)
                            {
                                if ((numOfPlayers == 2))
                                {
                                    continue;
                                }
                            }
                            if (mvar < diffs[ivar])
                            {
                                pathVars[ivar][mvar + (52 - diffs[ivar])].x = movePath[mvar].Left;
                                pathVars[ivar][mvar + (52 - diffs[ivar])].y = movePath[mvar].Top;
                            }
                            else
                            {
                                pathVars[ivar][mvar - diffs[ivar] + 1].x = movePath[mvar].Left;
                                pathVars[ivar][mvar - diffs[ivar] + 1].y = movePath[mvar].Top;
                            }
                        }
                    }
                }
                mvar++;
            }

            for (int mvar = 1; mvar <= 5; )
            {
                if (true) //rs_indexB != -1
                {
                    for (int ivar = 1; ivar <= 4; ivar++)
                    {
                        if ((numOfPlayers == 2))
                        {
                            if (ivar == 3 || ivar == 4)
                            {
                                continue;
                            }
                        }

                        int rs_index = shapeContainer1.Shapes.IndexOfKey("rectangleShapeH" + ivar + "" + mvar);
                        if (rs_index != -1) //rs_indexB != -1
                        {
                            colorSpMovePath[ivar][mvar] = (RectangleShape)shapeContainer1.Shapes.get_Item(rs_index);

                            pathVars[ivar][mvar + 50].x = colorSpMovePath[ivar][mvar].Left;
                            pathVars[ivar][mvar + 50].y = colorSpMovePath[ivar][mvar].Top;
                        }
                    }
                    mvar++;
                }
            }

            pathVars[1][56].x = 310; pathVars[1][56].y = 330;
            pathVars[2][56].x = 250; pathVars[2][56].y = 290;
            if ((numOfPlayers == 4))
            {
                pathVars[3][56].x = 290; pathVars[3][56].y = 270;
                pathVars[4][56].x = 350; pathVars[4][56].y = 310;
            }

            for (int jvar = 1; jvar <= numOfPlayers; jvar++)
            {
                beadBtnVars[jvar][1].BringToFront(); //TODO
            }

            diceRoll = initializeRoll();
            btnROLL.BackColor = Color.SkyBlue; //TODO

            //ovalShapes initialize
            int os_indexVal;
            for (int ivar = 1; ivar <= 3; ivar++)
            {
                for (int jvar = 1; jvar <= 3; jvar++)
                {
                    //plates[ivar, jvar] = new Button();
                    os_indexVal = shapeContainer1.Shapes.IndexOfKey("ovalShape" + ivar + "" + jvar);
                    diceBeads[((ivar - 1) * 3) + jvar - 1] = (OvalShape)shapeContainer1.Shapes.get_Item(os_indexVal);
                }
            }
        }

        /**
         * Method to initialize the DICE ROLL
         * */
        private int initializeRoll()
        {
            moveSequence[0] = new int();
            moveSequence[1] = new int();

            if (Configuration.firstMoveColorCode == 1)//Blue
            {
                moveSequence = new int[] { 1, 3};
                diceRoll = 0;
            }
            if (Configuration.firstMoveColorCode == 3)//Green
            {
                moveSequence = new int[] { 3, 1 };
                diceRoll = 0;
            }
            return diceRoll;
        }

        /**
         * Rolls the dice to next Player
         * */
        private void rollNext()
        {
            diceRoll = (diceRoll + 1) % numOfPlayers;
            textBox3.Text += "ROLL - " + diceRoll + "\r\n";
        }

        /**
         * ACTION - dice button Click
         * */
        private void btnRoll_Click(object sender, EventArgs e)
        {
            btnROLL.Enabled = false;
            diceValueRandomVar = new Random();
            currentDiceValue = diceValueRandomVar.Next(1, 7);
            textBox1.Text = currentDiceValue.ToString();

            setdice(currentDiceValue);

            switch (moveSequence[diceRoll])
            {
                case 1://blue to move
                    textBox3.Text += "Blue to move, Val - " + currentDiceValue + ",";
                    disableBeadBtn(beadBtnVars[diceRoll + 1]); //disable except this one
                    if (fmove[moveSequence[diceRoll]] == 0) //??-neverMovedTillNow
                        {
                            //textBox3.Text += "Blue 1st move";
                            firstMove(1, beadBtnVars[diceRoll + 1], beadVars[diceRoll + 1]);
                            btnROLL.Enabled = true;
                        }
                        else
                        {
                            if ((numbeadsopen[diceRoll + 1] == 1) && (currentDiceValue != 6)) // if only one bead open and val!=6 ..AutoMove
                            { autoMoveBlue(currentDiceValue); }
                            else
                            {
                                enableBeadBtns(currentDiceValue, beadBtnVars[diceRoll + 1], beadVars[diceRoll + 1]);
                                disableBeadBtn(beadBtnVars[diceRoll + 1]);
                                //indicate Blue Should be making a move now
                                //indicateNextColorToMove(); TODO
                            }
                        }
                    break;
                case 3: //green to move
                    textBox3.Text += "Green to move, Val - " + currentDiceValue + ",";
                    disableBeadBtn(beadBtnVars[diceRoll + 1]);
                    if (fmove[moveSequence[diceRoll]] == 0)
                    {
                        //textBox3.Text += "Green 1st move";
                        firstMove(3, beadBtnVars[diceRoll + 1], beadVars[diceRoll + 1]);
                        btnROLL.Enabled = true;

                    }
                    else
                    {
                        if ((numbeadsopen[diceRoll + 1] == 1) && (currentDiceValue != 6)) // ...Auto Move
                        { movegreen(currentDiceValue); }
                        else
                        {
                            enableBeadBtns(currentDiceValue, beadBtnVars[diceRoll + 1], beadVars[diceRoll + 1]);
                            disableBeadBtn(beadBtnVars[diceRoll + 1]);
                            //indicate Green Should be making a move now
                            //indicateNextColorToMove(); TODO
                        }
                    }
                    break;
            }


        }

        /**
         * Method to disable all beads that should not be moving the current move
         * */
        private void disableBeadBtn(Button[] button)
        {
            foreach(Button[] btnA in beadBtnVars)
            {
                if (btnA != button && (btnA !=null))
                {
                    for (int i = 1; i < btnA.Length; i++)
                    {
                        btnA[i].Enabled = false;
                    }
                }
            }
        }

        //private void indicateNextColorToMove()
        //{
        //    switch (diceRoll)
        //    {
        //        case 1: rectangleShapeBorder.BorderColor = Color.SkyBlue;
        //            rectangleShapeDice.BackColor = Color.DodgerBlue;

        //            rectangleShapeGB.BorderColor = Color.ForestGreen;
        //            rectangleShapeBB.BorderColor = Color.Black;
        //            break;
        //        case 3: 
        //            rectangleShapeBorder.BorderColor = Color.Green;
        //            rectangleShapeDice.BackColor = Color.ForestGreen;

        //            rectangleShapeGB.BorderColor = Color.Black;
        //            rectangleShapeBB.BorderColor = Color.DodgerBlue;
        //            break;
        //    }
        //}

        /**
         * Method to enable all beads that can move the current move
         * */
        private void enableBeadBtns(int diceVal, Button[] beadBtns, Bead[] bBeadBtns)
        {
            //Enable All, if dice says 6
            if (diceVal == 6)
            {
                for (int i = 1; i <= 4; i++)
                {
                    beadBtns[i].Enabled = true;
                }
            }
            else
            {
                //Enable only unlocked beads
                for (int i = 1; i <= 4; i++)
                {
                    if (bBeadBtns[i].status == 1)
                    {
                        beadBtns[i].Enabled = true;
                    }
                    else
                    {
                        beadBtns[i].Enabled = false;
                    }
                }

            }
        }

        /**
         * Method to set DICE for current Value
         * */
        private void setdice(int Val)
        {
            for (osl = 0; osl <= 8; osl++)
            {
                diceBeads[osl].BackColor = Color.Red;
            }
            switch (Val){
                case 1:
                    ovalShape11.Visible = false;
                    ovalShape12.Visible = false;
                    ovalShape13.Visible = false;
                    ovalShape21.Visible = false;
                    ovalShape22.Visible = true;
                    ovalShape23.Visible = false;
                    ovalShape31.Visible = false;
                    ovalShape32.Visible = false;
                    ovalShape33.Visible = false;
                    break;
                case 2:
                    ovalShape11.Visible = true;
                    ovalShape12.Visible = false;
                    ovalShape13.Visible = false;
                    ovalShape21.Visible = false;
                    ovalShape22.Visible = false;
                    ovalShape23.Visible = false;
                    ovalShape31.Visible = false;
                    ovalShape32.Visible = false;
                    ovalShape33.Visible = true;
                    break;
                case 3:
                    ovalShape11.Visible = true;
                    ovalShape12.Visible = false;
                    ovalShape13.Visible = false;
                    ovalShape21.Visible = false;
                    ovalShape22.Visible = true;
                    ovalShape23.Visible = false;
                    ovalShape31.Visible = false;
                    ovalShape32.Visible = false;
                    ovalShape33.Visible = true;
                    break;
                case 4:
                    ovalShape11.Visible = true;
                    ovalShape12.Visible = false;
                    ovalShape13.Visible = true;
                    ovalShape21.Visible = false;
                    ovalShape22.Visible = false;
                    ovalShape23.Visible = false;
                    ovalShape31.Visible = true;
                    ovalShape32.Visible = false;
                    ovalShape33.Visible = true;
                    break;
                case 5:
                    ovalShape11.Visible = true;
                    ovalShape12.Visible = false;
                    ovalShape13.Visible = true;
                    ovalShape21.Visible = false;
                    ovalShape22.Visible = true;
                    ovalShape23.Visible = false;
                    ovalShape31.Visible = true;
                    ovalShape32.Visible = false;
                    ovalShape33.Visible = true;
                    break;
                case 6:
                    ovalShape11.Visible = true;
                    ovalShape12.Visible = false;
                    ovalShape13.Visible = true;
                    ovalShape21.Visible = true;
                    ovalShape22.Visible = false;
                    ovalShape23.Visible = true;
                    ovalShape31.Visible = true;
                    ovalShape32.Visible = false;
                    ovalShape33.Visible = true;
                    break;


            }
            for (osl = 0; osl <= 8; osl++)
            {
                if (diceBeads[osl].BackColor == Color.Black)
                {
                    diceBeads[osl].Visible = false;
                }
            }
        }

        //private void resetdice()
        //{
        //    ovalShape11.BackColor = Color.Black;
        //    ovalShape12.BackColor = Color.Black;
        //    ovalShape13.BackColor = Color.Black;
        //    ovalShape21.BackColor = Color.Black;
        //    ovalShape22.BackColor = Color.Black;
        //    ovalShape23.BackColor = Color.Black;
        //    ovalShape31.BackColor = Color.Black;
        //    ovalShape32.BackColor = Color.Black;
        //    ovalShape33.BackColor = Color.Black;
        //}

        /**
         * Method for Auto-Move for Blue colored Beads
         **/
        private void autoMoveBlue(int val)
        {
            textBox3.Text += "auto-move blue";
            if (beadVars[diceRoll + 1][1].status == 1) //beadVars[ROLL + 1]
            {

                if ((beadVars[diceRoll + 1][1].loc + val) == 55)
                {
                    if ((val == 6) || (val == 1))
                    {
                        beadVars[diceRoll + 1][1].loc = beadVars[diceRoll + 1][1].loc + 1;
                        beadBtnVars[diceRoll + 1][1].Left = pathVars[diceRoll + 1][beadVars[diceRoll + 1][1].loc].x; //pathVars[ROLL + 1]
                        beadBtnVars[diceRoll + 1][1].Top = pathVars[diceRoll + 1][beadVars[diceRoll + 1][1].loc].y;
                        //ROLL = ROLL + 1;
                    }
                }
                else if (((beadVars[diceRoll + 1][1].loc + val) <= 56) && (val != 6))
                {
                    for (int mvar = 0; mvar < val; mvar++)
                    {
                        textBox3.Text += "TM" + (mvar+1);
                        beadVars[diceRoll + 1][1].loc = beadVars[diceRoll + 1][1].loc + 1;
                        beadBtnVars[diceRoll + 1][1].Left = pathVars[diceRoll + 1][beadVars[diceRoll + 1][1].loc].x;
                        beadBtnVars[diceRoll + 1][1].Top = pathVars[diceRoll + 1][beadVars[diceRoll + 1][1].loc].y;
                        for (long tvar = 0; tvar < 5000000L; tvar++) ;
                        //Thread.Sleep(100);
                    }
                }
                else if (((beadVars[diceRoll + 1][1].loc + val) <= 56) && (val == 6))
                {
                    beadVars[diceRoll + 1][1].loc = beadVars[diceRoll + 1][1].loc + val;
                    beadBtnVars[diceRoll + 1][1].Left = pathVars[diceRoll + 1][beadVars[diceRoll + 1][1].loc].x;
                    beadBtnVars[diceRoll + 1][1].Top = pathVars[diceRoll + 1][beadVars[diceRoll + 1][1].loc].y;

                }
                else if (((beadVars[diceRoll + 1][1].loc + val) > 56))
                {
                    //ROLL = 2;
                }
            }
            updateroll();
            unlock();
            if (val != 6)
            {
                beadBtnVars[diceRoll + 1][1].Enabled = false; beadBtnVars[diceRoll + 1][2].Enabled = false; beadBtnVars[diceRoll + 1][3].Enabled = false; beadBtnVars[diceRoll + 1][4].Enabled = false;
            }
        }

        /**
         * Method for Auto-Move for Green colored Beads
         **/
        private void movegreen(int val)
        {
            textBox3.Text += "auto-move green";
            if (beadVars[diceRoll + 1][1].status == 1)
                if ((beadVars[diceRoll + 1][1].loc + val) == 55)
                {
                    if ((val == 6) || (val == 1))
                    {
                        beadVars[diceRoll + 1][1].loc = beadVars[diceRoll + 1][1].loc + 1;
                        beadBtnVars[diceRoll + 1][1].Left = pathVars[diceRoll + 1][beadVars[diceRoll + 1][1].loc].x; //beadVars[ROLL + 1]
                        beadBtnVars[diceRoll + 1][1].Top = pathVars[diceRoll + 1][beadVars[diceRoll + 1][1].loc].y;
                        //ROLL = ROLL + 1;
                    }
                }
                else if (((beadVars[diceRoll + 1][1].loc + val) <= 56) && (val != 6))
                {
                    for (int mvar = 0; mvar < val; mvar++)
                    {
                        textBox3.Text += "TM" + (mvar + 1);
                        beadVars[diceRoll + 1][1].loc = beadVars[diceRoll + 1][1].loc + 1;
                        beadBtnVars[diceRoll + 1][1].Left = pathVars[diceRoll + 1][beadVars[diceRoll + 1][1].loc].x;
                        beadBtnVars[diceRoll + 1][1].Top = pathVars[diceRoll + 1][beadVars[diceRoll + 1][1].loc].y;
                        for (long tvar = 0; tvar < 5000000L; tvar++) ;
                        //Thread.Sleep(100);
                    }
                }
                else if (((beadVars[diceRoll + 1][1].loc + val) <= 56) && (val == 6))
                {
                    beadVars[diceRoll + 1][1].loc = beadVars[diceRoll + 1][1].loc + val;
                    beadBtnVars[diceRoll + 1][1].Left = pathVars[diceRoll + 1][beadVars[diceRoll + 1][1].loc].x;
                    beadBtnVars[diceRoll + 1][1].Top = pathVars[diceRoll + 1][beadVars[diceRoll + 1][1].loc].y;

                }
                else if (((beadVars[diceRoll + 1][1].loc + val) > 56))
                {
                    //ROLL = 1;
                }
            updateroll();
            unlock();
            if (val != 6)
            {
                beadBtnVars[diceRoll + 1][1].Enabled = false; beadBtnVars[diceRoll + 1][2].Enabled = false; beadBtnVars[diceRoll + 1][3].Enabled = false; beadBtnVars[diceRoll + 1][4].Enabled = false;
            }
        }

        //reflect who should be making the move now
        private void updateroll()
        {
            rollNext();
            textBox3.Text += "ms : " + moveSequence[diceRoll] + ",";
            if (moveSequence[diceRoll] == 3)
            {
                textBox2.Text = "Green";
                btnROLL.BackColor = Color.Green;
            }
            else if (moveSequence[diceRoll] == 1) //BLUE
            {
                textBox2.Text = "Blue";
                btnROLL.BackColor = Color.SkyBlue;
            }
        }

        public void unlock()
        {
            btnROLL.Enabled = true;
            for (dr = 1; dr < 5; dr++)
            {
                beadBtnVars[diceRoll + 1][dr].Enabled = true;
            }
        }

        /**
         * Method for maving FIRST Move for any colored Bead
         * */
        private void firstMove(int colourCode, Button[] beadBtns, Bead[] BeadBtns) // to replace firstMove<Color>
        {
            if ((currentDiceValue == 6) || (currentDiceValue == 1)) //Can open first bead
            {
                textBox3.Text += "1st move";
                fmove[moveSequence[diceRoll]] = 1;
                if (BeadBtns[1].status == 0)
                {
                    BeadBtns[1].status = 1;
                    BeadBtns[1].loc = 1;
                    beadBtns[1].Left = pathVars[diceRoll + 1][BeadBtns[1].loc].x;
                    beadBtns[1].Top = pathVars[diceRoll + 1][BeadBtns[1].loc].y;
                    beadBtns[1].Enabled = true;
                    numbeadsopen[diceRoll + 1] = numbeadsopen[diceRoll + 1] + 1; //TODO
                }
            }
            else
            {
                //TODO next player should move
                //rollNext();
                updateroll();
            }
        }

        /**
         * Method 1 - For common button Click Methods for the Grid of Buttons
         * This returns the Number value of button eg: 23 for button23
         **/
        public int getButtonNumber(object sender)
        {
            Button b = sender as Button;
            String s = b.Name;
            s = s.Replace("button", "");
            return Convert.ToInt32(s);
        }

        /**
         * Method 2 - For common button Click Methods for the Grid of Buttons
         * This is the click middleware- Click of button executes this,
         * which calls the generic Click method, providing Number value
         * of the button which was clicked by using Method 1.
         **/
        private void buttonAB_Click(object sender, EventArgs e)
        {
            button_Click(getButtonNumber(sender));
        }

        /**
         * Method 3 - For common button Click Methods for the Grid of Buttons
         * This is the actual code that shuld be executed when any of the 
         * button from the Grid of Button is clicked.
         **/
        private void button_Click(int p)
        {
            {
                performAction(p / 10, p % 10);
            }
        }

        private void performAction(int colourCode, int beadId)
        {
            if ((beadVars[diceRoll + 1][beadId].status == 1))
            {
                if ((beadVars[diceRoll + 1][beadId].loc + currentDiceValue) == 55)
                {
                    if ((currentDiceValue == 6) || (currentDiceValue == 1))
                    {
                        beadVars[diceRoll + 1][beadId].loc = beadVars[diceRoll + 1][beadId].loc + 1;
                        beadBtnVars[diceRoll + 1][beadId].Left = pathVars[diceRoll + 1][beadVars[diceRoll + 1][beadId].loc].x;
                        beadBtnVars[diceRoll + 1][beadId].Top = pathVars[diceRoll + 1][beadVars[diceRoll + 1][beadId].loc].y;
                        updateroll(); unlock();
                    }
                }
                else if (((beadVars[diceRoll + 1][beadId].loc + currentDiceValue) <= 56) && (currentDiceValue != 6))
                {
                    beadVars[diceRoll + 1][beadId].loc = beadVars[diceRoll + 1][1].loc + currentDiceValue;
                    beadBtnVars[diceRoll + 1][beadId].Left = pathVars[diceRoll + 1][beadVars[diceRoll + 1][beadId].loc].x;
                    beadBtnVars[diceRoll + 1][beadId].Top = pathVars[diceRoll + 1][beadVars[diceRoll + 1][beadId].loc].y;
                    updateroll(); unlock();
                }
                else if (((beadVars[diceRoll + 1][beadId].loc + currentDiceValue) <= 56) && (currentDiceValue == 6))
                {
                    beadVars[diceRoll + 1][beadId].loc = beadVars[diceRoll + 1][beadId].loc + currentDiceValue;
                    beadBtnVars[diceRoll + 1][beadId].Left = pathVars[diceRoll + 1][beadVars[diceRoll + 1][beadId].loc].x;
                    beadBtnVars[diceRoll + 1][beadId].Top = pathVars[diceRoll + 1][beadVars[diceRoll + 1][beadId].loc].y;
                    updateroll(); unlock();
                }
                else if (((beadVars[diceRoll + 1][beadId].loc + currentDiceValue) > 56))
                {
                    //ROLL = 2;
                }

                //User Has Moved
                //if Val was 6, then user has to roll dice again 
                //else change ROLL value to indicate the next player in line to Move
                if (currentDiceValue == 6)
                {
                    //doNothing, user will rerolldice
                    disableBeadBtn(beadBtnVars[diceRoll + 1]);
                }
                else
                {
                    disableBeadBtn(beadBtnVars[diceRoll + 1]);
                    updateroll(); unlock();
                }
            }
            else if (((beadVars[diceRoll + 1][beadId].status == 0) && (beadVars[diceRoll + 1][beadId].loc == -5)))
            {
                if ((currentDiceValue == 6) || (currentDiceValue == 1)) //Can open first bead
                {
                    textBox3.Text += "1st move";
                    fmove[moveSequence[diceRoll]] = 1;
                    if (beadVars[diceRoll + 1][beadId].status == 0)
                    {
                        beadVars[diceRoll + 1][beadId].status = 1;
                        beadVars[diceRoll + 1][beadId].loc = 1;
                        beadBtnVars[diceRoll + 1][beadId].Left = pathVars[diceRoll + 1][beadVars[diceRoll + 1][beadId].loc].x;
                        beadBtnVars[diceRoll + 1][beadId].Top = pathVars[diceRoll + 1][beadVars[diceRoll + 1][beadId].loc].y;
                        beadBtnVars[diceRoll + 1][beadId].Enabled = true;
                        numbeadsopen[diceRoll + 1] = numbeadsopen[diceRoll + 1] + 1; //TODO
                    }
                    unlock();
                }
            }
        }

        class Path
        {
            public int x;
            public int y;

            public Path()
            {
                x = new int();
                x = 0;
                y = new int();
                y = 0;
            }
        }

        class Bead
        { 
            public int loc;
            public int status;

            public Bead()
            {
                loc = -5;
                status = 0;
            }
        }
    }

    public class Constants
    {
        public static int ZERO = 0;
        public static int BLUE = 1;
        public static int RED = 2;
        public static int GREEN = 3;
        public static int YELLOW = 4;
    }

    public class Configuration
    {
        public static int firstMoveColorCode = 1;
        public static int currentPlayerCountConfig = 2;
        public static int[][] playerColorConfigs = 
            new int[][] { null,
                          null,
                          new int[] { Constants.ZERO, Constants.BLUE, Constants.GREEN },
                          null,
                          new int[] { Constants.ZERO, Constants.RED, Constants.GREEN, Constants.YELLOW, Constants.BLUE}
            };

    }
}
