using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Employee
{   
    public partial class Form1 : Form
    { 
        //creating a class for storing piece data for undo functionality 
        class PiecesData
        {

            public int initialRowIndex;// Old index
            public int initialColumnIndex;// old index
            public int currentRowIndex;// new index
            public int currentColumnIndex;//new index
            public int previousPieceNumber;// jo bhi piece number waha tha vo isme ayega
            public int pieceNumber;// jo move hua
            public Image previousImage; // old piece Image 
            public Image currentImage;// 
            public bool whiteRightRookMoved;
            public bool whiteLeftRookMoved;
            public bool blackRightRookMoved;
            public bool blackLeftRookMoved;
            public PiecesData(int ir,int ic,int cr,int cc,int ppn,int pieceNo,Image pi,Image ci,bool wrrm,bool wlrm,bool brrm,bool blrm)
            {
                initialRowIndex = ir;
                initialColumnIndex = ic;
                currentRowIndex=cr;
                currentColumnIndex=cc;
                previousPieceNumber = ppn;
                pieceNumber = pieceNo;
                previousImage = pi;
                currentImage=ci;
                whiteLeftRookMoved = wlrm;
                whiteRightRookMoved = wrrm;
                blackRightRookMoved = brrm;
                blackLeftRookMoved = blrm;


            }

        }
        class CircularStack
        {
            public int top = 0;
            public PiecesData[] stack=new PiecesData[10];
            public int count=0;
            public void Push(PiecesData p)
            {
               
                if (top == 10) top = 0;
                stack[top] = p;
                top++;
                if (count < 10) count++;
                  
            } 
            public PiecesData Pop()
            {
                if (count == 0) return null;
                if (top == 0) top = 10;
                top--;
                count--;
                return stack[top];
            }

            public void ClearStack()
            {
                top = 0;
                count = 0;
            }

        }
        //Color.FromArgb(kk123, 164, 90); // green
        //    Color.FromArgb(kk230 ,231 ,216);
        //Color.FromArgb(255, 169, 255); pink
        // 107, 186, 227 // blue

        CircularStack stack = new CircularStack();
        Color buttonColor1 = Color.FromArgb(123, 164, 90);
        Color buttonColor2 = Color.FromArgb(230, 231, 216);

        Color yellowColor = Color.FromArgb(255, 245, 157);// yellow color
        

        int count = 0;
        int whiteKingMoveCount = 0;
        int blackKingMoveCount = 0;
        int whiteKilledCount = 0;
        int blackKilledCount = 0;
        int[] whiteKilledPiecesNumber = new int[15];
        int[] blackKilledPiecesNumber = new int[15];
        int[] possibleRowIndex = new int[8];
        int[] possibleColIndex = new int[8];
        bool checkFromBothSide = false;
        bool isRookCheck = false;
        bool isBishopCheck = false;
        bool isKnightCheck = false;
        bool isPawnCheck=false;
        int RookRowIndex = 0;
        int RookColIndex = 0;
        int BishopRowIndex = 0;
        int BishopColIndex = 0;
        bool whiteChance = true;
        Button lastClick = null;
        int[,] pieces = new int[8, 8];
        char[,] color = new char[8, 8];
        int[,] coordinatesOfPossibleMoves = new int[8, 8];
        int[,] checkCoordinates=new int[8, 8];
        int row, col;
        int lastClickRow, lastClickCol;

        int WhiteKingRowIndex = 7;
        int WhiteKingColumnIndex = 4;

        int BlackKingRowIndex = 0;
        int BlackKingColumnIndex = 4;

        bool isRightWhiteRookMoved = false;
        bool isLeftWhiteRookMoved = false;
        bool isWhiteKingMoved = false;

        bool isRightBlackRookMoved = false;
        bool isLeftBlackRookMoved = false;
        bool isBlackKingMoved = false;

        Button[,] buttonArray = new Button[8, 8];
        PictureBox[] whitePictureBox = new PictureBox[15];
        PictureBox[] blackPictureBox = new PictureBox[15];
        


        //*********************************************
        public bool RookPathOfKingForCoordinates(int r/*7*/, int c/*3*/) //ye check krta hai ki raja waha per chl skta hia ya nhi mtlb usse check to nh lgegi waha per
        {


            int i;
            if (!whiteChance)//means piece is black
            {
                pieces[BlackKingRowIndex, BlackKingColumnIndex] = 0;
                //r=3 c=4
                i = r + 1;
                if (i < 8 && pieces[i, c] == 16)
                {
                    pieces[BlackKingRowIndex, BlackKingColumnIndex] = 6;
                    return true; // isse waha check krega ki waha raja to nh hai khi
                }
                for (; i < 8; i++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    //i=4
                    if (pieces[i, c] != 0 && pieces[i, c] / 10 == 0)// && pieces[r, i] != 6)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[i, c] != 0 && pieces[i, c] / 10 != 0)
                    {
                        if (pieces[i, c] == 15 || pieces[i, c] == 17)
                        {
                            pieces[BlackKingRowIndex, BlackKingColumnIndex] = 6;
                            return true;
                        }
                        break;
                    }
                    //MessageBox.Show("" + i);
                }
                i = r - 1;
                if (i >= 0 && pieces[i, c] == 16) {
                    pieces[BlackKingRowIndex, BlackKingColumnIndex] = 6;
                    return true; }

                for (; i >= 0; i--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, c] != 0 && pieces[i, c] / 10 == 0)// && pieces[r, i] != 6)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[i, c] != 0 && pieces[i, c] / 10 != 0)
                    {
                        if (pieces[i, c] == 15 || pieces[i, c] == 17)
                        {
                            pieces[BlackKingRowIndex, BlackKingColumnIndex] = 6;
                            return true;
                        }
                        break;
                    }
                }

                i = c + 1;
                if (i < 8 && pieces[r, i] == 16){
                 
                    pieces[BlackKingRowIndex, BlackKingColumnIndex] = 6;
                    return true; }
                for (; i < 8; i++)
                {
                    //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    //r=3 c=5
                    if (pieces[r, i] != 0 && pieces[r, i] / 10 == 0 )//&& pieces[r, i] != 6)
                    {


                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[r, i] != 0 && pieces[r, i] / 10 != 0)
                    {
                        if (pieces[r, i] == 15 || pieces[r, i] == 17)
                        {
                            pieces[BlackKingRowIndex, BlackKingColumnIndex] = 6;
//                            MessageBox.Show("kdfghjdshgkjfdhgjkkk**************************************");
                            return true;

                        }
                        break;
                    }
                }

                i = c - 1;
                if (i >= 0 && pieces[r, i] == 16)
                {
                    pieces[BlackKingRowIndex, BlackKingColumnIndex] = 6;
                    return true;
                }
                for (; i >= 0; i--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    //i=3 c=3
                    if (pieces[r, i] != 0 && pieces[r, i] / 10 == 0)// && pieces[r, i] != 6)
                    {

                        break;


                    }                                                   //piece opposite site ka h
                    else if (pieces[r, i] != 0 && pieces[r, i] / 10 != 0)
                    {
                        if (pieces[r, i] == 15 || pieces[r, i] == 17)
                        {
                            pieces[BlackKingRowIndex, BlackKingColumnIndex] = 6;
                            return true;
                        }
                        break;
                    }
                }

                pieces[BlackKingRowIndex, BlackKingColumnIndex] = 6;
            }

            else//means piece is white
            {
                //MessageBox.Show("White");
                //Row Wise Check
                pieces[WhiteKingRowIndex, WhiteKingColumnIndex] = 0;
                i = r + 1;
                if (i < 8 && pieces[i, c] == 6)
                {
                    pieces[WhiteKingRowIndex, WhiteKingColumnIndex] = 16;
                    return true;
                }
                for (; i < 8; i++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, c] != 0 && pieces[i, c] / 10 == 1 )// && pieces[r, i] != 16)//&& lastClickCol != WhiteKingColumnIndex)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                                                                        // else if(pieces[i, BlackKingColumnIndex] != 0 && pieces[i, BlackKingColumnIndex]/10 != 0)
                    else if (pieces[i, c] != 0 && pieces[i, c] / 10 != 1)
                    {
                        if (pieces[i, c] == 5 || pieces[i, c] == 7)
                        {
                            pieces[WhiteKingRowIndex, WhiteKingColumnIndex] = 16;
                            return true;
                        }
                        break;
                    }
                }
                //7//3
                i = r - 1;
                if (i >= 0 && pieces[i, c] == 6)
                {
                    pieces[WhiteKingRowIndex, WhiteKingColumnIndex] = 16;
                    return true;
                }
                for (; i >= 0; i--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    //i==6->5->4 c=3
                    if (pieces[i, c] != 0 && pieces[i, c] / 10 == 1)// && pieces[r, i] != 16)// && lastClickCol != WhiteKingColumnIndex)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[i, c] != 0 && pieces[i, c] / 10 != 1)
                    {
                        if (pieces[i, c] == 5 || pieces[i, c] == 7)
                        {
                            pieces[WhiteKingRowIndex, WhiteKingColumnIndex] = 16;
                            //MessageBox.Show("Yaha Pr");
                            return true;
                        }
                        break;
                    }
                }

                //7//4
                //Column Wise Check
                i = c + 1;
                if (i < 8 && pieces[r, i] == 6)
                {
                    pieces[WhiteKingRowIndex, WhiteKingColumnIndex] = 16;
                    return true;
                }
                for (; i < 8; i++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    //r=7//i=4->5
                    if (pieces[r, i] != 0 && pieces[r, i] / 10 == 1)// && pieces[r, i] != 16)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[r, i] != 0 && pieces[r, i] / 10 != 1)
                    {
                        if (pieces[r, i] == 5 || pieces[r, i] == 7)
                        {
                            pieces[WhiteKingRowIndex, WhiteKingColumnIndex] = 16;
                            return true;
                        }
                        break;
                    }
                }

                //7//3
                i = c - 1;
                if (i >= 0 && pieces[r, i] == 6)
                {
                    pieces[WhiteKingRowIndex, WhiteKingColumnIndex] = 16;
                    return true;
                }
                for (; i >= 0; i--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                   //r=7i=2
                    if (pieces[r, i] != 0 && pieces[r, i] / 10 == 1)// && pieces[r, i] != 16)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[r, i] != 0 && pieces[r, i] / 10 != 1)
                    {
                        if (pieces[r, i] == 5 || pieces[r, i] == 7)
                        {
                            pieces[WhiteKingRowIndex, WhiteKingColumnIndex] = 16;
                            return true;
                        }
                        break;
                    }
                }


                pieces[WhiteKingRowIndex, WhiteKingColumnIndex] = 16;
            }
            return false;
        }


        public bool BishopPathOfKingForCoordinates(int r, int c)
        {
            //7//3
            int i, j;
            if (!whiteChance)//means piece is black jo click hua hai
            {
                if ((r != 7 && c != 0) && (pieces[r + 1, c - 1] == 11))
                {
                    //MessageBox.Show("Whiteeeeeeeeeeeeeeeeeeeeeee");
                    return true;
                }
                if ((r != 7 && c != 7) && (pieces[r + 1, c + 1] == 11))
                {
                    //MessageBox.Show("Whiteeeeeeeeeeeeeeeeeeeeeee");
                    return true;
                }
                int BlackKingRowIndex = r;//4
                int BlackKingColumnIndex = c;//3
                i = BlackKingRowIndex + 1;
                j = BlackKingColumnIndex + 1;
                if (i < 8 && j < 8 && pieces[i, j] == 16) return true;
                for (; i < 8 && j < 8; i++, j++)
                {                                        //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h


                    //i=6 j=7
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 0 && pieces[i, j] != 6)
                    {
                        break;
                    }                                                //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 0)
                    {
                        if (pieces[i, j] == 18 || pieces[i, j] == 17)
                        {
                            return true;
                        }
                        break;
                    }
                    //MessageBox.Show("" + i);
                }
                i = BlackKingRowIndex - 1; j = BlackKingColumnIndex - 1;
                if (i >= 0 && j >= 0 && pieces[i, j] == 16) return true;
                for (; i >= 0 && j >= 0; i--, j--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    //i=3 j=4
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 0 && pieces[i, j] != 6)
                    {

                        break;
                    }                                                 //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 0)
                    {
                        if (pieces[i, j] == 18 || pieces[i, j] == 17)
                        {
                            return true;
                        }
                        break;
                    }
                }

                i = BlackKingRowIndex - 1; j = BlackKingColumnIndex + 1;
                if (i >= 0 && j <8 && pieces[i, j] == 16) return true;
                for (; i >= 0 && j < 8; i--, j++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h

                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 0 && pieces[i, j] != 6)
                    {

                        break;
                    }                                                  //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 0)
                    {
                        if (pieces[i, j] == 18 || pieces[i, j] == 17)
                        {
                            return true;
                        }
                        break;
                    }
                }

                i = BlackKingRowIndex +1 ; j = BlackKingColumnIndex - 1;
                if (i<8 && j >= 0 && pieces[i, j] == 16) return true;
                for (; i < 8 && j >= 0; i++, j--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h

                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 0 && pieces[i, j] != 6)
                    {

                        break;
                    }                                                 //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 0)
                    {
                        if (pieces[i, j] == 18 || pieces[i, j] == 17)
                        {
                            return true;
                        }
                        break;
                    }
                }



            }

            else//means piece is white
            {
                //7//3
                if ((r != 0 && c != 0) && (pieces[r - 1, c - 1] == 1))
                {
                    //MessageBox.Show("Whiteeeeeeeeeeeeeeeeeeeeeee");   
                    return true;
                }
                if ((r != 0 && c != 7) && (pieces[r - 1, c + 1] == 1))
                {
                    //MessageBox.Show("Whiteeeeeeeeeeeeeeeeeeeeeee");
                    return true;
                }
                //Row Wise Check
                int WhiteKingRowIndex = r;//6
                int WhiteKingColumnIndex = c;//1
                i = WhiteKingRowIndex + 1; j = WhiteKingColumnIndex + 1;
                if (i < 8 && j < 8 && pieces[i, j] == 6) return true;
                for (; i < 8 && j < 8; i++, j++)
                {           //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1 && pieces[i, j] != 16)
                    {

                        break;
                    }                                                 //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 1)
                    {
                        if (pieces[i, j] == 8 || pieces[i, j] == 7)
                        {
                            //MessageBox.Show("If ka 1st loop else if");
                            return true;
                        }
                        break;
                    }
                    //MessageBox.Show("" + i);
                }
                i = WhiteKingRowIndex - 1; j = WhiteKingColumnIndex - 1;
                if (i >= 0 && j >= 0 && pieces[i, j] == 6) return true;
                for (; i >= 0 && j >= 0; i--, j--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    //6//2
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1 && pieces[i, j] != 16)
                    {

                        break;
                    }                                                 //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 1)
                    {
                        if (pieces[i, j] == 8 || pieces[i, j] == 7)
                        {
                            //MessageBox.Show("If ka 2nd loop else if");
                            return true;
                        }
                        break;
                    }
                }
                i = WhiteKingRowIndex - 1; j = WhiteKingColumnIndex + 1;
                if (i >= 0 && j <8 && pieces[i, j] == 6) return true;
                for (i = WhiteKingRowIndex - 1, j = WhiteKingColumnIndex + 1; i >= 0 && j < 8; i--, j++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                                                                //if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1 && lastClickCol != j && lastClickRow != i)

                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1 && pieces[i, j] != 16)
                    {

                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 1)
                    {
                        if (pieces[i, j] == 8 || pieces[i, j] == 7)
                        {
                            //MessageBox.Show("If ka 3rd loop else if");
                            return true;
                        }
                        break;
                    }
                }
                i = WhiteKingRowIndex + 1; j = WhiteKingColumnIndex - 1;
                if (i < 8 && j >= 0 && pieces[i, j] == 6) return true;

                for (i = WhiteKingRowIndex + 1, j = WhiteKingColumnIndex - 1; i < 8 && j >= 0; i++, j--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1 && pieces[i, j] != 16)
                    {

                        break;
                    }                                                  //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 1)
                    {
                        if (pieces[i, j] == 8 || pieces[i, j] == 7)
                        {
                            //MessageBox.Show("If ka 4th loop else if");
                            return true;
                        }
                        break;
                    }
                }



            }
            return false;
        }

        public void setOneBetweenKingAndRook() // yaha function one rkhta hai king or jaha se check lgi hai uske bich
        {
            int r, c;
            count = 0;
            if (whiteChance)
            { // white chance
                if (WhiteKingRowIndex == RookRowIndex)//both on same row
                {
                    int i = WhiteKingRowIndex;
                    int j = WhiteKingColumnIndex;
                    if (j > RookColIndex)//rook left side hai 
                    {
                        j--;
                        for (; j >= RookColIndex; j--)
                        {
                            checkCoordinates[i, j] = 1;
                            possibleRowIndex[count] = i;
                            possibleColIndex[count] = j;
                            count++;
                        }
                    }
                    else//rook right side hai
                    {
                        j++;
                        for (; j <= RookColIndex; j++)
                        {
                            checkCoordinates[i, j] = 1;
                            possibleRowIndex[count] = i;
                            possibleColIndex[count] = j;
                            count++;
                        }
                    }
                }
                else//both on same col
                {
                    int i = WhiteKingRowIndex;//5
                    int j = WhiteKingColumnIndex;//5
                    if (i > RookRowIndex)//rook uper| side hai 
                    {
                        i--;
                        for (; i >= RookRowIndex; i--)
                        {
                            checkCoordinates[i, j] = 1;
                            possibleRowIndex[count] = i;
                            possibleColIndex[count] = j;
                            count++;
                        }
                    }
                    else//rook niche side hai
                    {
                        i++;
                        for (; i <= RookRowIndex; i++)
                        {
                            checkCoordinates[i, j] = 1;
                            possibleRowIndex[count] = i;
                            possibleColIndex[count] = j;
                            count++;
                        }
                    }
                }


            }
            else
            {// black chance 
             
                if (BlackKingRowIndex == RookRowIndex)//both on same row
                {
                    int i = BlackKingRowIndex;
                    int j = BlackKingColumnIndex;
                    if (j > RookColIndex)//rook left side hai 
                    {
                        j--;
                        for (; j >= RookColIndex; j--)
                        {
                            checkCoordinates[i, j] = 1;
                            possibleRowIndex[count] = i;
                            possibleColIndex[count] = j;
                            count++;
                        }
                    }
                    else//rook right side hai
                    {
                        j++;
                        for (; j <= RookColIndex; j++)
                        {
                            checkCoordinates[i, j] = 1;
                            possibleRowIndex[count] = i;
                            possibleColIndex[count] = j;
                            count++;
                        }
                    }
                }
                else//both on same col
                {
                    int i = BlackKingRowIndex;
                    int j = BlackKingColumnIndex;
                    if (i > RookRowIndex)//rook left side hai 
                    {
                        i--;
                        for (; i >= RookRowIndex; i--)
                        {
                            checkCoordinates[i, j] = 1;
                            possibleRowIndex[count] = i;
                            possibleColIndex[count] = j;
                            count++;
                        }
                    }
                    else//rook right side hai
                    {
                        i++;
                        for (; i <= RookRowIndex; i++)
                        {
                            checkCoordinates[i, j] = 1;
                            possibleRowIndex[count] = i;
                            possibleColIndex[count] = j;
                            count++;
                        }
                    }
                }
            }

        }
        public void setOneBetweenKingAndBishop()// yaha function one rkhta hai king or jaha se check lgi hai uske bich
        {
            count = 0;
            if (whiteChance)
            {

                if (WhiteKingRowIndex > BishopRowIndex) //esak matlab raja niche side hai bishop ya qeen uppar side hai
                {
                    if (WhiteKingColumnIndex < BishopColIndex)//eska matlab raja niche aur left side hai
                    {
                        int r = WhiteKingRowIndex-1;
                        int c = WhiteKingColumnIndex+1;
                        for (; r >= BishopRowIndex; r--, c++)
                        {
                            checkCoordinates[r, c] = 1;
                            possibleRowIndex[count] = r;
                            possibleColIndex[count] = c;
                            count++;
                        }
                    }
                    else// iska mtlb raja niche or right side hai
                    {
                        int r = WhiteKingRowIndex-1;
                        int c = WhiteKingColumnIndex-1;
                        for (; r >= BishopRowIndex; r--, c--)
                        {
                            checkCoordinates[r, c] = 1;
                            possibleRowIndex[count] = r;
                            possibleColIndex[count] = c;
                            count++;
                        }
                    }
                }
                else //eska mtlb raja upar side hai aue queen ya bishop niche side hai
                {
                    if (WhiteKingColumnIndex < BishopColIndex)//eska matlab raja upar aur left side hai
                    {
                        int r = WhiteKingRowIndex+1;
                        int c = WhiteKingColumnIndex+1;
                        for (; r <= BishopRowIndex; r++, c++)
                        {
                            checkCoordinates[r, c] = 1;
                            possibleRowIndex[count] = r;
                            possibleColIndex[count] = c;
                            count++;
                        }
                    }
                    else// iska mtlb raja upar or right side hai
                    {
                        int r = WhiteKingRowIndex+1;
                        int c = WhiteKingColumnIndex-1;
                        for (; r <= BishopRowIndex; r++, c--)
                        {

                            checkCoordinates[r, c] = 1;
                            possibleRowIndex[count] = r;
                            possibleColIndex[count] = c;
                            count++;
                        }
                    }
                }
            }
            else //black chance
            {
                
                
                if (BlackKingRowIndex > BishopRowIndex) //esak matlab raja niche side hai bishop ya qeen uppar side hai
                {
                    if (BlackKingColumnIndex < BishopColIndex)//eska matlab raja niche aur left side hai
                    {
                        int r = BlackKingRowIndex-1;
                        int c = BlackKingColumnIndex+1;
                        for (; r >= BishopRowIndex; r--, c++)
                        {
                            checkCoordinates[r, c] = 1;
                            possibleRowIndex[count] = r;
                            possibleColIndex[count] = c;
                            count++;
                        }
                    }
                    else// iska mtlb raja niche or right side hai
                    {
                        int r = BlackKingRowIndex - 1;
                        int c = BlackKingColumnIndex - 1;
                        for (; r >= BishopRowIndex; r--, c--)
                        {
                            checkCoordinates[r, c] = 1;
                            possibleRowIndex[count] = r;
                            possibleColIndex[count] = c;
                            count++;
                        }
                    }
                }
                else //eska mtlb raja upar side hai aue queen ya bishop niche side hai
                {
                    if (BlackKingColumnIndex < BishopColIndex)//eska matlab raja upar aur left side hai
                    {
                        int r = BlackKingRowIndex + 1;
                        int c = BlackKingColumnIndex + 1;
                        for (; r <= BishopRowIndex; r++, c++)
                        {
                            checkCoordinates[r, c] = 1;
                            possibleRowIndex[count] = r;
                            possibleColIndex[count] = c;
                            count++;
                        }
                    }
                    else// iska mtlb raja upar or right side hai
                    {
                        int r = BlackKingRowIndex + 1;
                        int c = BlackKingColumnIndex - 1;
                        for (; r <= BishopRowIndex; r++, c--)
                        {
                            checkCoordinates[r, c] = 1;
                            possibleRowIndex[count] = r;
                            possibleColIndex[count] = c;
                            count++;
                        }
                    }
                }

            }
        }
      public bool KnightPathOfKingForCoordinates(int rr,int cc,int p)
        {


            //7//3
            int r = rr + 2;//9
            int c = cc + 1;//4

            

            if (r < 8 && c < 8)
            {                           //
                
                if (pieces[r, c] == 9 || pieces[r, c] == 19)
                {
                    if (pieces[r, c] / 10 != p / 10) return true;
                }

            }
            c = cc - 1;//2
            if (r < 8 && c >= 0)
            {
                if (pieces[r, c] == 9 || pieces[r, c] == 19)
                {
                    if (pieces[r, c] / 10 != p / 10) return true;
                }

            }
            r = rr - 2;//5
            if (r >= 0 && c >= 0)
            {
                if (pieces[r, c] == 9 || pieces[r, c] == 19)
                {
                    if (pieces[r, c] / 10 != p / 10) return true;
                }
            }
            c = cc + 1;//4
            if (r >= 0 && c < 8)
            {
                if (pieces[r, c] == 9 || pieces[r, c] == 19)
                {
                    if (pieces[r, c] / 10 != p / 10) return true;
                }

            }

            r = rr + 1;//8
            c =cc + 2;//5
            if (r < 8 && c < 8)
            {
                if (pieces[r, c] == 9 || pieces[r, c] == 19)
                {
                    if (pieces[r, c] / 10 != p / 10) return true;
                }
            }
            c = cc - 2;//1
            if (r < 8 && c >= 0)
            {
                if (pieces[r, c] == 9 || pieces[r, c] == 19)
                {
                    if (pieces[r, c] / 10 != p / 10) return true;
                }
            }
            r = rr - 1;//6
            if (r >= 0 && c >= 0)
            {
                if (pieces[r, c] == 9 || pieces[r, c] == 19)
                {
                    if (pieces[r, c] / 10 != p / 10) return true;
                }
            }
            c = cc + 2;//5
            if (r >= 0 && c < 8)
            {
                if (pieces[r, c] == 9 || pieces[r, c] == 19)
                {
                    if (pieces[r, c] / 10 != p / 10) return true;
                }
            }
            return false;
        }

        //*********************************************
        public bool PawnPathForMate()
        {
            if (whiteChance)
            {// white ke liye check krna hai

                for(int i = 0; i < count; i++)
                {
                    int r = possibleRowIndex[i];
                    int c = possibleColIndex[i];
                                            // mtlb waha per khud ka pawn h // or waha per koi piece hai to v usko maar skta hai
                    if(r!=7 && c != 0 && pieces[r + 1, c - 1] == 11 && pieces[r,c]!=0)
                    {
                        return false;
                    }
                    
                    if (r != 7  && pieces[r + 1, c] == 11 && pieces[r, c] == 0)
                    {
                        return false;
                    }
                    if ((r+2) == 6 && pieces[r + 2, c] == 11 && pieces[r, c] == 0)
                    {
                        return false;
                    }

                    if (r != 7 && c != 7 && pieces[r + 1, c + 1] == 11 && pieces[r, c] != 0)
                    {
                        return false;
                    }

                }
            
            }
            else
            {// black k liye check krenge
                for (int i = 0; i < count; i++)
                {
                    int r = possibleRowIndex[i];
                    int c = possibleColIndex[i];
                    if (r != 0 && c != 0 && pieces[r - 1, c - 1] == 1 && pieces[r, c] != 0)
                    {
                        return false;
                    }

                    if (r != 0 && pieces[r - 1, c] == 1 && pieces[r, c] == 0)
                    {
                        return false;
                    }
                    if (r == 3 && pieces[r - 2, c] == 1 && pieces[r, c] == 0)
                    {
                        return false;
                    }

                    if (r != 0 && c != 7 && pieces[r - 1, c + 1] == 1 && pieces[r, c] != 0)
                    {
                        return false;
                    }

                }
            }
            return true;
        }
        public bool RookPathForMate()
        {
            if (whiteChance)
            {
                int r, c;
                for(int i = 0; i < count; i++)
                {
                    r = possibleRowIndex[i];
                    c = possibleColIndex[i];
                    int rr,cc;
                    for (rr = r - 1; rr >= 0 && pieces[rr, c] == 0; rr--)
                    {

                    }
                    if (rr >= 0 && (pieces[rr, c] == 15 || pieces[rr, c] == 17) )
                    {
                        return false;
                    }
                    for (rr = r + 1; rr < 8 && pieces[rr, c] == 0; rr++)
                    {

                    }
                    if (rr < 8 && (pieces[rr, c] == 15 || pieces[rr, c] == 17))
                    {
                        return false;
                    }
                    for(cc=c-1;cc>=0 && pieces[r, cc] == 0; cc--)
                    {

                    }
                    if(cc>=0 && (pieces[r, cc] == 15 || pieces[r, cc] == 17))
                    {
                        return false;
                    }
                    for (cc = c + 1; cc < 8 && pieces[r, cc] == 0; cc++)
                    {

                    }
                    if (cc < 8 && (pieces[r, cc] == 15 || pieces[r, cc] == 17))
                    {
                        return false;
                    }

                }
            }
            else
            {
                int r, c;
                for (int i = 0; i < count; i++)
                {
                    r = possibleRowIndex[i];
                    c = possibleColIndex[i];
                    int rr, cc;
                    for (rr = r - 1; rr >= 0 && pieces[rr, c] == 0; rr--)
                    {

                    }
                    if (rr >= 0 && (pieces[rr, c] == 5 || pieces[rr, c] == 7))
                    {
                        return false;
                    }
                    for (rr = r + 1; rr < 8 && pieces[rr, c] == 0; rr++)
                    {

                    }
                    if (rr < 8 && (pieces[rr, c] == 5 || pieces[rr, c] == 7))
                    {
                        return false;
                    }
                    for (cc = c - 1; cc >= 0 && pieces[r, cc] == 0; cc--)
                    {

                    }
                    if (cc >= 0 && (pieces[r, cc] == 5 || pieces[r, cc] == 7))
                    {
                        return false;
                    }
                    for (cc = c + 1; cc < 8 && pieces[r, cc] == 0; cc++)
                    {

                    }
                    if (cc < 8 && (pieces[r, cc] == 5 || pieces[r,cc]==7))
                    {
                        return false;
                    }

                }
            }
            return true;
        }
        public bool BishopPathForMate()
        {
            if (whiteChance)
            {
                for(int i = 0; i < count; i++)
                {
                    int r = possibleRowIndex[i];
                    int c = possibleColIndex[i];
                    int rr, cc;
                    for(rr=r-1,cc = c-1;rr>=0 && cc>=0 && pieces[rr,cc] == 0; rr--, cc--)
                    {

                    }
                    if(rr>=0 && cc>=0 && (pieces[rr,cc] == 18 || pieces[rr, cc] == 17))
                    {
                        return false;
                    }

                    for(rr = r+1,cc=c+1;rr<8 && cc<8 && pieces[rr,cc] == 0;rr++, cc++)
                    {

                    }
                    if(rr<8 && cc<8 && (pieces[rr,cc] == 18 || pieces[rr,cc] == 17))
                    {
                        return false;
                    }

                    for(rr=r-1,cc=c+1;rr>=0 && cc<8 && pieces[rr, cc] == 0; rr--, cc++)
                    {

                    }
                    if(rr>=0 && cc<8 && (pieces[rr,cc] == 18 || pieces[rr,cc] == 17))
                    {
                        return false;
                    }

                    for (rr = r + 1, cc = c - 1; rr < 8 && cc >= 0 && pieces[rr, cc] == 0; rr++, cc--)
                    {

                    }
                    if (cc >= 0 && rr < 8 && (pieces[rr, cc] == 18 || pieces[rr, cc] == 17))
                    {
                        return false;
                    }

                }
            }
            else
            {// black chance
                for (int i = 0; i < count; i++)
                {
                    int r = possibleRowIndex[i];
                    int c = possibleColIndex[i];
                    int rr, cc;
                    for (rr = r - 1, cc = c - 1; rr >= 0 && cc >= 0 && pieces[rr, cc] == 0; rr--, cc--)
                    {

                    }
                    if (rr >= 0 && cc >= 0 && (pieces[rr, cc] == 8 || pieces[rr, cc] == 7))
                    {
                        return false;
                    }

                    for (rr = r + 1, cc = c + 1; rr < 8 && cc < 8 && pieces[rr, cc] == 0; rr++, cc++)
                    {

                    }
                    if (rr < 8 && cc < 8 && (pieces[rr, cc] == 8 || pieces[rr, cc] == 7))
                    {
                        return false;
                    }

                    for (rr = r - 1, cc = c + 1; rr >= 0 && cc < 8 && pieces[rr, cc] == 0; rr--, cc++)
                    {

                    }
                    if (rr >= 0 && cc < 8 && (pieces[rr, cc] == 8 || pieces[rr, cc] == 7))
                    {
                        return false;
                    }

                    for (rr = r + 1, cc = c - 1; rr < 8 && cc >= 0 && pieces[rr, cc] == 0; rr++, cc--)
                    {

                    }
                    if (cc >= 0 && rr < 8 && (pieces[rr, cc] == 8 || pieces[rr, cc] == 7))
                    {
                        return false;
                    }

                }
            }

            return true;
        }
        public bool KnightPathForMate()
        {
            if (whiteChance)
            {
                for(int i = 0; i < count; i++)
                {
                    int r = possibleRowIndex[i];
                    int c = possibleColIndex[i];
                    int rr, cc;
                    rr = r - 2;
                    cc = c - 1;
                    if(rr>=0 && cc>=0 && pieces[rr, cc] == 19)
                    {
                        return false;
                    }
                    rr = r - 2;
                    cc = c + 1;
                    if (rr >= 0 && cc < 8 && pieces[rr, cc] == 19)
                    {
                        return false;
                    }
                    rr = r + 2;
                    cc = c + 1;
                    if (rr < 8 && cc < 8 && pieces[rr, cc] == 19)
                    {
                        return false;
                    }
                    rr = r + 2;
                    cc = c - 1;
                    if (cc >= 0 && rr < 8 && pieces[rr, cc] == 19)
                    {
                        return false;
                    }

                    cc = c - 2;
                    rr = r - 1;
                    if (rr >= 0 && cc >= 0 && pieces[rr, cc] == 19)
                    {
                        return false;
                    }
                    cc = c - 2;
                    rr = r + 1;
                    if (cc >= 0 && rr < 8 && pieces[rr, cc] == 19)
                    {
                        return false;
                    }
                    cc = c + 2;
                    rr = r + 1;
                    if (rr < 8 && cc < 8 && pieces[rr, cc] == 19)
                    {
                        return false;
                    }
                    cc = c + 2;
                    rr = r - 1;
                    if (rr >= 0 && cc < 8 && pieces[rr, cc] == 19)
                    {
                        return false;
                    }


                }
                
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    int r = possibleRowIndex[i];
                    int c = possibleColIndex[i];
                    int rr, cc;
                    rr = r - 2;
                    cc = c - 1;
                    if (rr >= 0 && cc >= 0 && pieces[rr, cc] == 9)
                    {
                        return false;
                    }
                    rr = r - 2;
                    cc = c + 1;
                    if (rr >= 0 && cc < 8 && pieces[rr, cc] == 9)
                    {
                        return false;
                    }
                    rr = r + 2;
                    cc = c + 1;
                    if (rr < 8 && cc < 8 && pieces[rr, cc] == 9)
                    {
                        return false;
                    }
                    rr = r + 2;
                    cc = c - 1;
                    if (cc >= 0 && rr < 8 && pieces[rr, cc] ==9)
                    {
                        return false;
                    }

                    cc = c - 2;
                    rr = r - 1;
                    if (rr >= 0 && cc >= 0 && pieces[rr, cc] == 9)
                    {
                        return false;
                    }
                    cc = c - 2;
                    rr = r + 1;
                    if (cc >= 0 && rr < 8 && pieces[rr, cc] == 9)
                    {
                        return false;
                    }
                    cc = c + 2;
                    rr = r + 1;
                    if (rr < 8 && cc < 8 && pieces[rr, cc] == 9)
                    {
                        return false;
                    }
                    cc = c + 2;
                    rr = r - 1;
                    if (rr >= 0 && cc < 8 && pieces[rr, cc] == 9)
                    {
                        return false;
                    }


                }
            }

            return true;
        }
        public bool isKingMovementPossible()
        {
            if (whiteChance)
            {//white ke liye check krna h 
                int r = WhiteKingRowIndex;
                int c = WhiteKingColumnIndex;
                int cc = 0;
                if (c != 0 &&(pieces[r,c-1]==0 || pieces[r,c-1]/10!=1) && RookPathOfKingForCoordinates(r, c - 1) == false &&  BishopPathOfKingForCoordinates(r, c - 1) == false && KnightPathOfKingForCoordinates(r, c - 1, 16) == false)
                {// iska mtlb raja wha chl skta
                    return true;
                }
                if (c != 7 &&(pieces[r,c+1]==0 || pieces[r,c+1]/10!=1 )&& RookPathOfKingForCoordinates(r, c + 1) == false && BishopPathOfKingForCoordinates(r, c + 1) == false && KnightPathOfKingForCoordinates(r, c + 1, 16) == false)
                {
                    return true;
                }
                if (c != 7 && r != 7 &&(pieces[r+1,c+1]==0 || pieces[r+1,c+1]/10!=1 )&& RookPathOfKingForCoordinates(r + 1, c + 1) == false && BishopPathOfKingForCoordinates(r + 1, c + 1) == false && KnightPathOfKingForCoordinates(r + 1, c + 1, 16) == false)
                {
                   return true;
                }
                if (r != 7 &&(pieces[r+1,c]==0 || pieces[r+1,c]/10!=1 )&& RookPathOfKingForCoordinates(r + 1, c) ==false &&  BishopPathOfKingForCoordinates(r + 1, c) ==false &&  KnightPathOfKingForCoordinates(r + 1, c, 16) == false)
                {
                  return true;
                }
                if (r != 7 && c != 0 &&(pieces[r+1,c-1]==0 || pieces[r+1,c-1]/10!=1 )&& RookPathOfKingForCoordinates(r + 1, c - 1) ==false &&  BishopPathOfKingForCoordinates(r + 1, c - 1) ==false &&  KnightPathOfKingForCoordinates(r + 1, c - 1, 16) == false)
                {
                   return true;
                }

                if (r != 0 && c != 7 &&(pieces[r-1,c+1]==0 || pieces[r-1,c+1]/10!=1 )&& RookPathOfKingForCoordinates(r - 1, c + 1) == false && BishopPathOfKingForCoordinates(r - 1, c + 1) ==false &&  KnightPathOfKingForCoordinates(r - 1, c + 1, 16) == false)
                {
                   return true;
                }
                if (r != 0 &&(pieces[r-1,c]==0 || pieces[r-1,c]/10!=1) && RookPathOfKingForCoordinates(r - 1, c) ==false &&  BishopPathOfKingForCoordinates(r - 1, c) ==false &&  KnightPathOfKingForCoordinates(r - 1, c, 16) == false)
                {
                   return true;
                }
                if (r != 0 && c != 0 &&(pieces[r-1,c-1]==0 || pieces[r-1,c-1]/10!=1) && RookPathOfKingForCoordinates(r - 1, c - 1) == false && BishopPathOfKingForCoordinates(r - 1, c - 1) == false && KnightPathOfKingForCoordinates(r - 1, c - 1, 16) == false)
                {
                  return true;
                }

                }

            else
            {//black ke liye check krna h 
                int r = BlackKingRowIndex;
                int c = BlackKingColumnIndex;
                int cc = 0;
                if (c != 0 &&(pieces[r,c-1]==0 || pieces[r,c-1]/10!=0) && RookPathOfKingForCoordinates(r, c - 1) ==false &&  BishopPathOfKingForCoordinates(r, c - 1) == false && KnightPathOfKingForCoordinates(r, c - 1, 6) ==  false)
                {
                    return true;
                }
                if (c != 7 &&(pieces[r,c+1]==0 || pieces[r,c+1]/10!=0) && RookPathOfKingForCoordinates(r, c + 1) ==false &&  BishopPathOfKingForCoordinates(r, c + 1) ==false &&  KnightPathOfKingForCoordinates(r, c + 1, 6) ==  false)
                {
                    return true;
                }
                if (c != 7 && r != 7 && (pieces[r+1,c+1]==0 || pieces[r+1,c+1]/10!=0) && RookPathOfKingForCoordinates(r + 1, c + 1) == false && BishopPathOfKingForCoordinates(r + 1, c + 1) ==false &&  KnightPathOfKingForCoordinates(r + 1, c + 1, 6) ==  false)
                {
                    return true;
                }
                if (r != 7 &&(pieces[r+1,c]==0 || pieces[r+1,c]/10!=0) && RookPathOfKingForCoordinates(r + 1, c) ==false &&  BishopPathOfKingForCoordinates(r + 1, c) ==false &&  KnightPathOfKingForCoordinates(r + 1, c, 6) == false)
                {
                    return true;
                }
                if (r != 7 && c != 0 &&(pieces[r+1,c-1]==0 || pieces[r+1,c-1]/10!=0) && RookPathOfKingForCoordinates(r + 1, c - 1) == false && BishopPathOfKingForCoordinates(r + 1, c - 1) ==false &&  KnightPathOfKingForCoordinates(r + 1, c - 1, 6) ==  false)
                {
                    return true;
                }

                if (r != 0 && c != 7 &&(pieces[r-1,c+1]==0 || pieces[r-1,c+1]/10!=0) && RookPathOfKingForCoordinates(r - 1, c + 1) ==false &&  BishopPathOfKingForCoordinates(r - 1, c + 1) == false && KnightPathOfKingForCoordinates(r - 1, c + 1, 6) == false)
                {
                    return true;
                }
                if (r != 0 &&(pieces[r-1,c]==0 || pieces[r-1,c]/10!=0) && RookPathOfKingForCoordinates(r - 1, c) == false && BishopPathOfKingForCoordinates(r - 1, c) == false && KnightPathOfKingForCoordinates(r - 1, c, 6) == false)
                {
                    return true;
                }
                if (r != 0 && c != 0 &&(pieces[r-1,c-1]==0 || pieces[r-1,c-1]/10!=0) && RookPathOfKingForCoordinates(r - 1, c - 1) ==false &&  BishopPathOfKingForCoordinates(r - 1, c - 1) ==false &&  KnightPathOfKingForCoordinates(r - 1, c - 1, 6) ==  false)
                {
                    return true;
                }

               
            }


            return false;
        }
        public bool isCheckMate()
        {
            if (checkFromBothSide == false) // to sabka path dekhenge
            {
                if (isRookCheck) setOneBetweenKingAndRook();
                else if (isBishopCheck)
                {
                    
                    setOneBetweenKingAndBishop();
                }
                //bool pp = PawnPathForMate();
                //bool rp = RookPathForMate();
                //bool bp = BishopPathForMate();
                //bool kp = KnightPathForMate();

                //MessageBox.Show("Pawn -> "+pp.ToString() + " rook->" + rp.ToString()+" Bishop-> "+bp.ToString()+" knight-> "+kp.ToString());

                if(PawnPathForMate() && RookPathForMate() && BishopPathForMate() && KnightPathForMate())
                {
                    
                    
                        if (whiteChance)
                        {
                            MessageBox.Show("Black Won");
                        label1.Text = "Black Won";
                        pictureBox1.Image = Properties.Resources.blackking1;
                        panel1.Enabled = false;

                        }
                        else
                        {
                            MessageBox.Show("White Won");
                        label1.Text = "White Won";
                        pictureBox1.Image=Properties.Resources.whiteking2;
                        panel1.Enabled=false;
                        
                        }
                    
                }
                
            }

            return false;
        }
        void IsCheck()
        {
          int i=CheckRookPathOfKingForCheck();
            int j = CheckBishopPathOfKingForCheck();
            int k = CheckKnightPathOfKingForCheck();
            if ((i > 0 && j > 0)|| (k > 0 && j > 0)|| (i > 0 && k > 0)) //check from both side
            {
                checkFromBothSide = true;
                //set boolean true
            }
            //MessageBox.Show(isRookCheck.ToString()+" <-Rook "+isBishopCheck.ToString()+" <-Bishoop"+isPawnCheck.ToString()+" <-Pawn Check");
            if (isBishopCheck || isKnightCheck || isRookCheck || isPawnCheck)
            {
                //MessageBox.Show("Check");
                if (isKingMovementPossible()==false) isCheckMate();
            }
        }





        //*********************************************


        //*********************************************************
        //          Path Guide And Movement

        private void RookPath(int p)
        {
            if (checkFromBothSide) return;

            if ((isBishopCheck || isRookCheck) && isKnightCheck) return;

            else
            {//raja ko check nhi hai
                if (isKnightCheck)
                {

                }
                else if (isRookCheck)
                {
                    setOneBetweenKingAndRook();// isse waha bich me 1 rakh dega raja or jaha se check lgi hai uske bich
                }
                else if (isBishopCheck && isPawnCheck==false)
                {
                    setOneBetweenKingAndBishop();// isse waha bich me 1 rakh dega raja or jaha se check lgi hai uske bich
                }

                int x = 0;
                int y = 0;
                int i = 0;
                int j = 0;

                if (!isRookCheck) i = RookPathOfKing();
                if(!isBishopCheck ) j = BishopPathOfKing();
                    //MessageBox.Show("Issue in moving" + i);
                if (i > 0 && j > 0) return;



                //if (!isRookCheck || !isBishopCheck)//isse pta chalega ki ya to bishop rook ya fir queen se check h agr kisi ek se check hai to hume pta krna padega ki dusra piece hatane se raja ko check nhi lagega isliye restricted path nikalenge
                //{
                //    //check kr rahe hai isko hatane se raja ko check lag rahi hai kya
                //   if(!isRookCheck) i = RookPathOfKing();
                //   if(!isBishopCheck) j = BishopPathOfKing();
                //    //MessageBox.Show("Issue in moving" + i);
                //    if (i > 0 && j > 0) return;
                //}

                if ((i>0 || j>0) && (isKnightCheck||isRookCheck||isBishopCheck)) return;//iska mtlb raja ko check hai or usko hatane se bhi raja ko check lgegi to rook chl hi nh skta

                else if ((i > 0 || j > 0) )
                {
                    if (i > 0) RestrictedPathOfRookAndQueen(i);

                }
                else
                {
                    //   MessageBox.Show("Issue in movinggskjgjlknvngidsgm vglkjdfhgnm,");
                    x = lastClickRow - 1;

                    for (; x >= 0; x--)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[x, lastClickCol] == 0)
                        {
                            if (pieces[x,lastClickCol]!=0) break;
                            continue;
                        }
                        if (pieces[x, lastClickCol] != 0)
                        {
                            if (pieces[x, lastClickCol] / 10 != p / 10)
                            {
                                buttonArray[x, lastClickCol].BackColor = yellowColor;
                                coordinatesOfPossibleMoves[x, lastClickCol] = 1;
                            }
                            break;

                        }
                        coordinatesOfPossibleMoves[x, lastClickCol] = 1;
                        buttonArray[x, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                    }
                    x = lastClickRow + 1;

                    for (; x < 8; x++)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[x, lastClickCol] == 0)
                        {
                            if (pieces[x, lastClickCol] != 0) break;
                            continue;
                        }
                        if (pieces[x, lastClickCol] != 0)
                        {
                            if (pieces[x, lastClickCol] / 10 != p / 10)
                            {
                                buttonArray[x, lastClickCol].BackColor = yellowColor;
                                coordinatesOfPossibleMoves[x, lastClickCol] = 1;
                            }
                            break;

                        }
                        coordinatesOfPossibleMoves[x, lastClickCol] = 1;
                        buttonArray[x, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                    }
                    //Loop for column wise moving of rook
                    y = lastClickCol - 1;
                    for (; y >= 0; y--)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[lastClickRow, y] == 0)
                        {
                            if (pieces[lastClickRow, y] != 0) break;
                            continue;
                        }
                        if (pieces[lastClickRow, y] != 0)
                        {
                            if (pieces[lastClickRow, y] / 10 != p / 10)
                            {
                                buttonArray[lastClickRow, y].BackColor = yellowColor;
                                coordinatesOfPossibleMoves[lastClickRow, y] = 1;
                            }
                            break;

                        }
                        coordinatesOfPossibleMoves[lastClickRow, y] = 1;
                        buttonArray[lastClickRow, y].BackgroundImage = Properties.Resources.dot6;
                    }
                    y = lastClickCol + 1;
                    for (; y < 8; y++)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[lastClickRow, y] == 0)
                        {
                            if (pieces[lastClickRow, y] != 0) break;
                            continue;
                        }
                        if (pieces[lastClickRow, y] != 0)
                        {
                            if (pieces[lastClickRow, y] / 10 != p / 10)
                            {
                                buttonArray[lastClickRow, y].BackColor = yellowColor;
                                coordinatesOfPossibleMoves[lastClickRow, y] = 1;
                            }
                            break;

                        }
                        coordinatesOfPossibleMoves[lastClickRow, y] = 1;
                        buttonArray[lastClickRow, y].BackgroundImage = Properties.Resources.dot6;
                    }
                }
            }
        }

        private void PawnPath(int p)
        {
            if (checkFromBothSide) return;
            if ((isBishopCheck || isRookCheck) && isKnightCheck) return;
            int i=0, j=0;
            
            if(!isRookCheck) i = RookPathOfKing();
            if(!isBishopCheck) j = BishopPathOfKing();
            if (i > 0 && j > 0) return;
            if (isKnightCheck)
            {

            }
            else if (isRookCheck)
            {
                setOneBetweenKingAndRook();// isse waha bich me 1 rakh dega raja or jaha se check lgi hai uske bich
            }
            else if (isBishopCheck && isPawnCheck == false)
            {
                setOneBetweenKingAndBishop();// isse waha bich me 1 rakh dega raja or jaha se check lgi hai uske bich
            }
            

            
            if (i > 0 || j > 0)
            {
                if (j > 0)
                {
                    if (p == 1)//black
                    {

                        if (lastClickRow != 7 && lastClickCol != 7 && pieces[lastClickRow + 1, lastClickCol + 1] == j )
                        {
                            if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[lastClickRow + 1, lastClickCol + 1] == 0)
                            {

                            }
                            else
                            {
                                coordinatesOfPossibleMoves[lastClickRow + 1, lastClickCol + 1] = 1;
                                buttonArray[lastClickRow + 1, lastClickCol + 1].BackColor = yellowColor;
                            }
                        }
                        else if (lastClickRow != 7 && lastClickCol != 0 && pieces[lastClickRow + 1, lastClickCol - 1] == j )
                        {
                            if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[lastClickRow + 1, lastClickCol - 1] == 0)
                            {

                            }
                            else
                            {
                                coordinatesOfPossibleMoves[lastClickRow + 1, lastClickCol - 1] = 1;
                                buttonArray[lastClickRow + 1, lastClickCol - 1].BackColor = yellowColor;
                            }
                        }
                    }
                    else//white
                    {
                        if (lastClickRow != 0 && lastClickCol != 7 && pieces[lastClickRow - 1, lastClickCol + 1] == j )
                        {
                            if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[lastClickRow - 1, lastClickCol + 1] == 0)
                            {

                            }
                            else
                            {
                                coordinatesOfPossibleMoves[lastClickRow - 1, lastClickCol + 1] = 1;
                                buttonArray[lastClickRow - 1, lastClickCol + 1].BackColor = yellowColor;
                            }
                        }
                        else if (lastClickRow != 0 && lastClickCol != 0 && pieces[lastClickRow - 1, lastClickCol - 1] == j )
                        {
                            if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[lastClickRow - 1, lastClickCol - 1] == 0)
                            {

                            }
                            else
                            {
                                coordinatesOfPossibleMoves[lastClickRow - 1, lastClickCol - 1] = 1;
                                buttonArray[lastClickRow - 1, lastClickCol - 1].BackColor = yellowColor;
                            }
                        }
                    }
                
                }//yaha se me likh raha hu
                if (i > 0)
                {
                    if (p == 1)
                    {
                        if (lastClickRow != 7 && pieces[lastClickRow + 1, lastClickCol] == 0)
                        {
                            if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[lastClickRow + 1, lastClickCol] == 0)
                            {

                            }
                            else if(BlackKingColumnIndex==lastClickCol)
                            {
                                // MessageBox.Show("Tu ko nhi chal raha");
                                coordinatesOfPossibleMoves[lastClickRow + 1, lastClickCol] = 1;
                                buttonArray[lastClickRow + 1, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                            }
                        }
                        if (lastClickRow == 1 && pieces[lastClickRow+1,lastClickCol]==0 && pieces[lastClickRow + 2, lastClickCol] == 0)
                        {
                            if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[lastClickRow + 2, lastClickCol] == 0)
                            {

                            }
                            else if (BlackKingColumnIndex == lastClickCol)
                            {
                                // MessageBox.Show("Tu ko nhi chal raha");
                                coordinatesOfPossibleMoves[lastClickRow + 2, lastClickCol] = 1;
                                buttonArray[lastClickRow + 2, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                            }
                        }

                    }
                    else
                    {
                        if (lastClickRow != 0 && pieces[lastClickRow - 1, lastClickCol] == 0)
                        {
                            if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[lastClickRow - 1, lastClickCol] == 0)
                            {

                            }
                            else if(WhiteKingColumnIndex==lastClickCol) // iska mtlb jisse check lgi hai vo or raja or jo chal rha hai vo tino ek line m h 
                            {
                                // MessageBox.Show("Tu ko nhi chal raha");
                                coordinatesOfPossibleMoves[lastClickRow - 1, lastClickCol] = 1;
                                buttonArray[lastClickRow - 1, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                            }
                        }
                        if (lastClickRow == 6 && pieces[lastClickRow - 1, lastClickCol] == 0 && pieces[lastClickRow - 2, lastClickCol] == 0)
                        {
                            if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[lastClickRow - 2, lastClickCol] == 0)
                            {

                            }
                            else if (WhiteKingColumnIndex == lastClickCol) // iska mtlb jisse check lgi hai vo or raja or jo chal rha hai vo tino ek line m h 
                            {
                                // MessageBox.Show("Tu ko nhi chal raha");
                                coordinatesOfPossibleMoves[lastClickRow - 2, lastClickCol] = 1;
                                buttonArray[lastClickRow - 2, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                            }
                        }
                    }
                }
                
            }
            else 
            { 
            int x = lastClickRow;
            int y;
                if (p == 1)
                {//black piece
                    //MessageBox.Show("Black Ka Piece CHalega");
                    if (x != 7  && pieces[x + 1, lastClickCol] == 0)
                    {
                        if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[x + 1, lastClickCol] == 0)
                        {

                        }
                        else
                        {
                            // MessageBox.Show("Tu ko nhi chal raha");
                            coordinatesOfPossibleMoves[x + 1, lastClickCol] = 1;
                            buttonArray[x + 1, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                        }
                    }
                    if (x == 1 && pieces[x + 1, lastClickCol] == 0 && pieces[x + 2, lastClickCol] == 0)
                    {
                        if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[x + 2, lastClickCol] == 0)
                        {

                        }
                        else
                        {
                            // MessageBox.Show("Tu ko nhi chal raha");
                            coordinatesOfPossibleMoves[x + 2, lastClickCol] = 1;
                            buttonArray[x + 2, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                        }
                    }
                    y = lastClickCol + 1;
                    if (y < 8 && pieces[x + 1, y] != 0 && pieces[x + 1, y] / 10 != p / 10)
                    {
                        if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[x + 1, y] == 0)
                        {

                        }
                        else
                        {
                            coordinatesOfPossibleMoves[x + 1, y] = 1;
                            buttonArray[x + 1, y].BackColor = yellowColor;
                        }
                    }
                    y = lastClickCol - 1;
                    if (y >= 0 && pieces[x + 1, y] != 0 && pieces[x + 1, y] / 10 != p / 10)
                    {
                        if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[x + 1, y] == 0)
                        {

                        }
                        else
                        {
                            coordinatesOfPossibleMoves[x + 1, y] = 1;
                            buttonArray[x + 1, y].BackColor = yellowColor;
                        }
                    }
                }
                else//white
                {
                    if (x!=0 && pieces[x - 1, lastClickCol] == 0)
                    {
                        if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[x - 1 , lastClickCol] == 0)
                        {

                        }
                        else
                        {
                            //MessageBox.Show("Tu ko nhi chal raha");
                            coordinatesOfPossibleMoves[x - 1, lastClickCol] = 1;
                            buttonArray[x - 1, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                        }
                    }
                    if (x == 6 && pieces[x -1, lastClickCol] == 0 && pieces[x - 2, lastClickCol] == 0)
                    {
                        if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[x - 2, lastClickCol] == 0)
                        {

                        }
                        else
                        {
                            //MessageBox.Show("Tu ko nhi chal raha");
                            coordinatesOfPossibleMoves[x - 2, lastClickCol] = 1;
                            buttonArray[x - 2, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                        }
                    }
                    y = lastClickCol + 1;
                    if (y < 8 && pieces[x - 1, y] != 0 && pieces[x - 1, y] / 10 != p / 10)
                    {
                        if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[x - 1, y] == 0)
                        {

                        }
                        else
                        {
                            coordinatesOfPossibleMoves[x - 1, y] = 1;
                            buttonArray[x - 1, y].BackColor = yellowColor;
                        }
                    }
                    y = lastClickCol - 1;
                    if (y >= 0 && pieces[x - 1, y] != 0 && pieces[x - 1, y] / 10 != p / 10)
                    {
                        if ((isKnightCheck || isBishopCheck || isRookCheck) && checkCoordinates[x - 1, y] == 0)
                        {

                        }
                        else
                        {
                            coordinatesOfPossibleMoves[x - 1, y] = 1;
                            buttonArray[x - 1, y].BackColor = yellowColor;
                        }
                    }
                }
            }
        }

        private void BishopPath(int p)
        {
            if (checkFromBothSide) return;
            if ((isBishopCheck || isRookCheck) && isKnightCheck) return;
            else
            {//raja ko check nhi hai
                if (isRookCheck)
                {
                    setOneBetweenKingAndRook();// isse waha bich me 1 rakh dega raja or jaha se check lgi hai uske bich
                }
                else if (isBishopCheck && isPawnCheck == false)
                {
                    setOneBetweenKingAndBishop();// isse waha bich me 1 rakh dega raja or jaha se check lgi hai uske bich
                }
                int x = 0;
                int y = 0;
                int i = 0;
                int j = 0;


                if (!isRookCheck) i = RookPathOfKing();
                if (!isBishopCheck) j = BishopPathOfKing();
                //MessageBox.Show("Issue in moving" + i);
                if (i > 0 && j > 0) return;


                //if (!isRookCheck || !isBishopCheck)//isse pta chalega ki ya to bishop rook ya fir queen se check h agr kisi ek se check hai to hume pta krna padega ki dusra piece hatane se raja ko check nhi lagega isliye restricted path nikalenge
                //{
                //    //check kr rahe hai isko hatane se raja ko check lag rahi hai kya
                //    i = RookPathOfKing();//7
                //    j = BishopPathOfKing();//
                //    //MessageBox.Show("Issue in moving" + i);
                //    if (i > 0 && j > 0) return;
                //}
                if ((i > 0 || j > 0) && (isKnightCheck || isRookCheck || isBishopCheck)) return;//iska mtlb raja ko check hai or usko hatane se bhi raja ko check lgegi to rook chl hi nh skta

                else if (i > 0 || j > 0)
                {
                    if (j > 0) RestrictedPathOfBishopAndQueen(j);

                }
                else
                {
                    for (x = lastClickRow - 1, y = lastClickCol - 1; x >= 0 && y >= 0; x--, y--)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[x, y] == 0)
                        {
                            if (pieces[x, y] != 0) break;
                            continue;
                        }
                        if (pieces[x, y] != 0)
                        {

                            if (pieces[x, y] / 10 != p / 10)
                            {
                                coordinatesOfPossibleMoves[x, y] = 1;
                                buttonArray[x, y].BackColor = yellowColor;
                            }
                            break;
                        }
                        coordinatesOfPossibleMoves[x, y] = 1;
                        buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                    }

                    //loop for lower right
                    for (x = lastClickRow + 1, y = lastClickCol + 1; x < 8 && y < 8; x++, y++)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[x, y] == 0)
                        {
                            if (pieces[x, y] != 0) break;
                            continue;
                        }
                        if (pieces[x, y] != 0)
                        {

                            if (pieces[x, y] / 10 != p / 10)
                            {
                                coordinatesOfPossibleMoves[x, y] = 1;
                                buttonArray[x, y].BackColor = yellowColor;
                            }
                            break;
                        }
                        coordinatesOfPossibleMoves[x, y] = 1;
                        buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                    }

                    //loop for lower left
                    for (x = lastClickRow + 1, y = lastClickCol - 1; x < 8 && y >= 0; x++, y--)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[x, y] == 0)
                        {
                            if (pieces[x, y] != 0) break;
                            continue;
                        }
                        if (pieces[x, y] != 0)
                        {
                            if (pieces[x, y] / 10 != p / 10)
                            {
                                coordinatesOfPossibleMoves[x, y] = 1;
                                buttonArray[x, y].BackColor = yellowColor;
                            }
                            break;
                        }
                        coordinatesOfPossibleMoves[x, y] = 1;
                        buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                    }

                    //loop for upper right
                    for (x = lastClickRow - 1, y = lastClickCol + 1; x >= 0 && y < 8; x--, y++)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[x, y] == 0)
                        {
                            if (pieces[x, y] != 0) break;
                            continue;
                        }
                        if (pieces[x, y] != 0)
                        {
                            if (pieces[x, y] / 10 != p / 10)
                            {
                                coordinatesOfPossibleMoves[x, y] = 1;
                                buttonArray[x, y].BackColor = yellowColor;
                            }
                            break;
                        }
                        coordinatesOfPossibleMoves[x, y] = 1;
                        buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                    }
                }
            }
        }

        private void QueenPath(int p)
        {

            if (checkFromBothSide) return;
            if ((isBishopCheck || isRookCheck) && isKnightCheck) return;
            else
            {//raja ko check nhi hai
                if (isRookCheck)
                {
                    setOneBetweenKingAndRook();// isse waha bich me 1 rakh dega raja or jaha se check lgi hai uske bich
                }
                else if (isBishopCheck && isPawnCheck == false)
                {
                    setOneBetweenKingAndBishop();// isse waha bich me 1 rakh dega raja or jaha se check lgi hai uske bich
                }
                int x = 0;
                int y = 0;
                int i = 0;
                int j = 0;


                if (!isRookCheck) i = RookPathOfKing();
                if (!isBishopCheck) j = BishopPathOfKing();
                //MessageBox.Show("Issue in moving" + i);
                if (i > 0 && j > 0) return;

                ////check kr rahe hai isko hatane se raja ko check lag rahi hai kya 
                //if (!isRookCheck || !isBishopCheck)
                //{
                //    i = RookPathOfKing();
                //    j = BishopPathOfKing();
                //    //MessageBox.Show("Issue in moving" + i);
                //    if (i > 0 && j > 0) return;
                //    //if (isRookCheck && j > 0) return;
                //    //if (isBishopCheck && i > 0) return;


                //}
                // if ((i > 0 || j > 0) && (isRookCheck || isBishopCheck)) return;// iska mtlb raja ko check hai or isko hatane ki wjh se bhi raja ko check lgega to ese m yaha move hi nhi kar skta


                if ((i > 0 || j > 0) && (isKnightCheck || isRookCheck || isBishopCheck)) return;//iska mtlb raja ko check hai or usko hatane se bhi raja ko check lgegi to rook chl hi nh skta
                else if (i > 0 || j > 0)
                {
                    if (i > 0) RestrictedPathOfRookAndQueen(i);
                    if (j > 0) RestrictedPathOfBishopAndQueen(j);
                }
                else
                {
                    // iska matlb queen free hai raja ko check nh lag rhi ab vo kahi per bhi chal skti h
                    for (x = lastClickRow - 1; x >= 0; x--)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[x, lastClickCol] == 0)
                        {
                            if (pieces[x, lastClickCol] != 0) break;
                            continue;
                        }
                        if (pieces[x, lastClickCol] != 0)
                        {
                            if (pieces[x, lastClickCol] / 10 != p / 10)
                            {
                                coordinatesOfPossibleMoves[x, lastClickCol] = 1;
                                buttonArray[x, lastClickCol].BackColor = yellowColor;

                            }
                            break;

                        }
                        coordinatesOfPossibleMoves[x, lastClickCol] = 1;
                        buttonArray[x, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                    }


                    for (x = lastClickRow + 1; x < 8; x++)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[x, lastClickCol] == 0)
                        {
                            if (pieces[x, lastClickCol] != 0) break;
                            continue;
                        }
                        if (pieces[x, lastClickCol] != 0)
                        {
                            if (pieces[x, lastClickCol] / 10 != p / 10)
                            {
                                coordinatesOfPossibleMoves[x, lastClickCol] = 1;
                                buttonArray[x, lastClickCol].BackColor = yellowColor;

                            }
                            break;

                        }
                        coordinatesOfPossibleMoves[x, lastClickCol] = 1;
                        buttonArray[x, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                    }
                    //Loop for column wise moving of rook
                    for (y = lastClickCol - 1; y >= 0; y--)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[lastClickRow, y] == 0)
                        {
                            if (pieces[lastClickRow, y] != 0) break;
                            continue;
                        }
                        if (pieces[lastClickRow, y] != 0)
                        {
                            if (pieces[lastClickRow, y] / 10 != p / 10)
                            {
                                coordinatesOfPossibleMoves[lastClickRow, y] = 1;
                                buttonArray[lastClickRow, y].BackColor = yellowColor;

                            }
                            break;

                        }
                        coordinatesOfPossibleMoves[lastClickRow, y] = 1;
                        buttonArray[lastClickRow, y].BackgroundImage = Properties.Resources.dot6;
                    }
                    for (y = lastClickCol + 1; y < 8; y++)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[lastClickRow, y] == 0)
                        {
                            if (pieces[lastClickRow, y] != 0) break;
                            continue;
                        }
                        if (pieces[lastClickRow, y] != 0)
                        {
                            if (pieces[lastClickRow, y] / 10 != p / 10)
                            {
                                coordinatesOfPossibleMoves[lastClickRow, y] = 1;
                                buttonArray[lastClickRow, y].BackColor = yellowColor;

                            }
                            break;

                        }
                        coordinatesOfPossibleMoves[lastClickRow, y] = 1;
                        buttonArray[lastClickRow, y].BackgroundImage = Properties.Resources.dot6;
                    }

                    //loop for upper left
                    for (x = lastClickRow - 1, y = lastClickCol - 1; x >= 0 && y >= 0; x--, y--)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[x, y] == 0)
                        {
                            if (pieces[x, y] != 0) break;
                            continue;
                        }
                        if (pieces[x, y] != 0)
                        {
                            if (pieces[x, y] / 10 != p / 10)
                            {
                                coordinatesOfPossibleMoves[x, y] = 1;
                                buttonArray[x, y].BackColor = yellowColor;
                            }
                            break;
                        }
                        coordinatesOfPossibleMoves[x, y] = 1;
                        buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                    }

                    //loop for lower right
                    for (x = lastClickRow + 1, y = lastClickCol + 1; x < 8 && y < 8; x++, y++)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[x, y] == 0)
                        {
                            if (pieces[x, y] != 0) break;
                            continue;
                        }
                        if (pieces[x, y] != 0)
                        {
                            if (pieces[x, y] / 10 != p / 10)
                            {
                                coordinatesOfPossibleMoves[x, y] = 1;
                                buttonArray[x, y].BackColor = yellowColor;
                            }
                            break;
                        }
                        coordinatesOfPossibleMoves[x, y] = 1;
                        buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                    }

                    //loop for lower left
                    for (x = lastClickRow + 1, y = lastClickCol - 1; x < 8 && y >= 0; x++, y--)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[x, y] == 0)
                        {
                            if (pieces[x, y] != 0) break;
                            continue;
                        }
                        if (pieces[x, y] != 0)
                        {
                            if (pieces[x, y] / 10 != p / 10)
                            {
                                coordinatesOfPossibleMoves[x, y] = 1;
                                buttonArray[x, y].BackColor = yellowColor;
                            }
                            break;
                        }
                        coordinatesOfPossibleMoves[x, y] = 1;
                        buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                    }

                    //loop for upper right
                    for (x = lastClickRow - 1, y = lastClickCol + 1; x >= 0 && y < 8; x--, y++)
                    {
                        if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[x, y] == 0)
                        {
                            if (pieces[x, y] != 0) break;
                            continue;
                        }
                        if (pieces[x, y] != 0)
                        {
                            if (pieces[x, y] / 10 != p / 10)
                            {
                                coordinatesOfPossibleMoves[x, y] = 1;
                                buttonArray[x, y].BackColor = yellowColor;
                            }
                            break;
                        }
                        coordinatesOfPossibleMoves[x, y] = 1;
                        buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                    }
                }
            }
        }

        //king esko bad me chalayenge
        private void KingPath(int p) 
        {
            // checking is castling possible
            if (isWhiteKingMoved == false && isLeftWhiteRookMoved == false)
            {// mtlb raja or left wala rook dono abhi tak move nahi hue hai 
                
                if (pieces[7, 3] == 0 && pieces[7, 2] == 0 && pieces[7, 1] == 0 && RookPathOfKingForCoordinates(7, 3) == false && BishopPathOfKingForCoordinates(7, 3) == false && KnightPathOfKingForCoordinates(7, 3, p) == false && RookPathOfKingForCoordinates(7, 2) == false && BishopPathOfKingForCoordinates(7, 2) == false && KnightPathOfKingForCoordinates(7, 2, p) == false)
                {
                    coordinatesOfPossibleMoves[7, 2] = 1;
                    buttonArray[7,2].BackgroundImage = Properties.Resources.dot6;
                    coordinatesOfPossibleMoves[7, 3] = 1;
                    buttonArray[7, 2].BackgroundImage = Properties.Resources.dot6;
                }

            }
            if (isWhiteKingMoved == false && isRightWhiteRookMoved == false)
            {// mtlb raja or right wala rook dono abhi tak move nahi hue hai 
                
                if (pieces[7, 5] == 0 && pieces[7, 6] == 0 && RookPathOfKingForCoordinates(7, 5) == false && BishopPathOfKingForCoordinates(7, 5) == false && KnightPathOfKingForCoordinates(7, 5, p) == false && RookPathOfKingForCoordinates(7, 6) == false && BishopPathOfKingForCoordinates(7, 6) == false && KnightPathOfKingForCoordinates(7, 6, p) == false)
                {
                    coordinatesOfPossibleMoves[7, 6] = 1;
                    buttonArray[7, 6].BackgroundImage = Properties.Resources.dot6;
                    coordinatesOfPossibleMoves[7, 5] = 1;
                    buttonArray[7, 5].BackgroundImage = Properties.Resources.dot6;
                }

            }
            // checking is castling possible
            if (isBlackKingMoved == false && isLeftBlackRookMoved == false)
            {// mtlb raja or left wala rook dono abhi tak move nahi hue hai 

                if (pieces[0, 3] == 0 && pieces[0, 2] == 0 && pieces[0, 1] == 0 && RookPathOfKingForCoordinates(0, 3) == false && BishopPathOfKingForCoordinates(0, 3) == false && KnightPathOfKingForCoordinates(0, 3, p) == false && RookPathOfKingForCoordinates(0, 2) == false && BishopPathOfKingForCoordinates(0, 2) == false && KnightPathOfKingForCoordinates(0, 2, p) == false)
                {
                    coordinatesOfPossibleMoves[0, 2] = 1;
                    buttonArray[0, 2].BackgroundImage = Properties.Resources.dot6;
                    coordinatesOfPossibleMoves[0, 3] = 1;
                    buttonArray[0, 2].BackgroundImage = Properties.Resources.dot6;
                }

            }
            if (isBlackKingMoved == false && isRightBlackRookMoved == false)
            {// mtlb raja or right wala rook dono abhi tak move nahi hue hai 

                if (pieces[0, 5] == 0 && pieces[0, 6] == 0 && RookPathOfKingForCoordinates(0, 5) == false && BishopPathOfKingForCoordinates(0, 5) == false && KnightPathOfKingForCoordinates(0, 5, p) == false && RookPathOfKingForCoordinates(0, 6) == false && BishopPathOfKingForCoordinates(0, 6) == false && KnightPathOfKingForCoordinates(0, 6, p) == false)
                {
                    coordinatesOfPossibleMoves[0, 6] = 1;
                    buttonArray[0, 6].BackgroundImage = Properties.Resources.dot6;
                    coordinatesOfPossibleMoves[0, 5] = 1;
                    buttonArray[0, 5].BackgroundImage = Properties.Resources.dot6;
                }

            }
            if (lastClickCol != 0 && pieces[lastClickRow, lastClickCol - 1] == 0)
            {
                
                if (RookPathOfKingForCoordinates(lastClickRow, lastClickCol - 1)==false && BishopPathOfKingForCoordinates(lastClickRow, lastClickCol - 1) == false && KnightPathOfKingForCoordinates(lastClickRow, lastClickCol - 1,p)==false)
                {//7 //3
                    

                    coordinatesOfPossibleMoves[lastClickRow, lastClickCol - 1] = 1;
                    buttonArray[lastClickRow, lastClickCol - 1].BackgroundImage = Properties.Resources.dot6;
                }
            }
            else if (lastClickCol != 0 && pieces[lastClickRow, lastClickCol - 1] / 10 != p / 10)
            {
               
                if (RookPathOfKingForCoordinates(lastClickRow, lastClickCol - 1) == false && BishopPathOfKingForCoordinates(lastClickRow, lastClickCol - 1) == false && KnightPathOfKingForCoordinates(lastClickRow, lastClickCol - 1, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow, lastClickCol - 1] = 1;
                    buttonArray[lastClickRow, lastClickCol - 1].BackColor = yellowColor;
                } 
            }

            
            if (lastClickCol != 7 && pieces[lastClickRow, lastClickCol + 1] == 0)
            {
                
                
                if (RookPathOfKingForCoordinates(lastClickRow, lastClickCol + 1) == false && BishopPathOfKingForCoordinates(lastClickRow, lastClickCol + 1) == false && KnightPathOfKingForCoordinates(lastClickRow, lastClickCol + 1, p) == false)
                {
                   
                    coordinatesOfPossibleMoves[lastClickRow, lastClickCol + 1] = 1;
                    buttonArray[lastClickRow, lastClickCol + 1].BackgroundImage = Properties.Resources.dot6;
                   
                }
            }
            else if (lastClickCol != 7 && pieces[lastClickRow, lastClickCol + 1] / 10 != p / 10)
            {
              
                if (RookPathOfKingForCoordinates(lastClickRow, lastClickCol + 1) == false && BishopPathOfKingForCoordinates(lastClickRow, lastClickCol + 1) == false && KnightPathOfKingForCoordinates(lastClickRow, lastClickCol + 1, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow, lastClickCol + 1] = 1;
                    buttonArray[lastClickRow, lastClickCol + 1].BackColor = yellowColor;
                }
            }


            if (lastClickRow != 7 && pieces[lastClickRow + 1, lastClickCol] == 0)
            {//5 6
                //MessageBox.Show((lastClickRow+1) + " " + (lastClickCol));
                if (RookPathOfKingForCoordinates(lastClickRow + 1, lastClickCol) == false && BishopPathOfKingForCoordinates(lastClickRow + 1, lastClickCol) == false && KnightPathOfKingForCoordinates(lastClickRow+1, lastClickCol, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow + 1, lastClickCol] = 1;
                    buttonArray[lastClickRow + 1, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                }
            }
            else if (lastClickRow != 7 && pieces[lastClickRow + 1, lastClickCol] / 10 != p / 10)
            {
               // MessageBox.Show((lastClickRow + 1) + " " + (lastClickCol));
                if (RookPathOfKingForCoordinates(lastClickRow+1, lastClickCol) == false && BishopPathOfKingForCoordinates(lastClickRow+1, lastClickCol) == false && KnightPathOfKingForCoordinates(lastClickRow+1, lastClickCol, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow + 1, lastClickCol] = 1;
                    buttonArray[lastClickRow + 1, lastClickCol].BackColor = yellowColor;
                }
            }


            if (lastClickRow != 0 && pieces[lastClickRow - 1, lastClickCol] == 0)
            {
               // MessageBox.Show((lastClickRow - 1) + " " + (lastClickCol));
                if (RookPathOfKingForCoordinates(lastClickRow - 1, lastClickCol) == false && BishopPathOfKingForCoordinates(lastClickRow - 1, lastClickCol) == false && KnightPathOfKingForCoordinates(lastClickRow-1, lastClickCol, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow - 1, lastClickCol] = 1;
                    buttonArray[lastClickRow - 1, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                }
            }
            else if (lastClickRow != 0 && pieces[lastClickRow - 1, lastClickCol] / 10 != p / 10)
            {
              //  MessageBox.Show((lastClickRow - 1) + " " + (lastClickCol));
                if (RookPathOfKingForCoordinates(lastClickRow - 1, lastClickCol ) == false && BishopPathOfKingForCoordinates(lastClickRow-1, lastClickCol) == false && KnightPathOfKingForCoordinates(lastClickRow-1, lastClickCol, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow - 1, lastClickCol] = 1;
                    buttonArray[lastClickRow - 1, lastClickCol].BackColor = yellowColor;
                }
            }


            if (lastClickRow != 0 && lastClickCol != 0 && pieces[lastClickRow - 1, lastClickCol - 1] == 0)
            {
                // MessageBox.Show((lastClickRow - 1) + " " + (lastClickCol-1));

                /*MessageBox.Show(lastClickCol - 1 + "<- col,row-> " + (lastClickRow-1));
                MessageBox.Show(RookPathOfKingForCoordinates(lastClickRow-1, lastClickCol - 1).ToString());
                MessageBox.Show(BishopPathOfKingForCoordinates(lastClickRow-1, lastClickCol - 1).ToString());
                MessageBox.Show(KnightPathOfKingForCoordinates(lastClickRow-1, lastClickCol - 1, p).ToString());*/
                if (RookPathOfKingForCoordinates(lastClickRow - 1, lastClickCol - 1) == false && BishopPathOfKingForCoordinates(lastClickRow - 1, lastClickCol - 1) == false && KnightPathOfKingForCoordinates(lastClickRow-1, lastClickCol - 1, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow - 1, lastClickCol - 1] = 1;
                    buttonArray[lastClickRow - 1, lastClickCol - 1].BackgroundImage = Properties.Resources.dot6;
                }
            }
            else if (lastClickRow != 0 && lastClickCol != 0 && pieces[lastClickRow - 1, lastClickCol - 1] / 10 != p / 10)
            {
              //  MessageBox.Show((lastClickRow - 1) + " " + (lastClickCol-1));
                if (RookPathOfKingForCoordinates(lastClickRow - 1, lastClickCol - 1) == false && BishopPathOfKingForCoordinates(lastClickRow - 1, lastClickCol - 1) == false && KnightPathOfKingForCoordinates(lastClickRow-1, lastClickCol - 1, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow - 1, lastClickCol - 1] = 1;
                    buttonArray[lastClickRow - 1, lastClickCol - 1].BackColor = yellowColor;
                }
            }

            if (lastClickRow != 7 && lastClickCol != 7 && pieces[lastClickRow + 1, lastClickCol + 1] == 0)
            {
                //MessageBox.Show((lastClickRow + 1) + " " + (lastClickCol+1));
                if (RookPathOfKingForCoordinates(lastClickRow + 1, lastClickCol + 1) == false && BishopPathOfKingForCoordinates(lastClickRow + 1, lastClickCol + 1) == false && KnightPathOfKingForCoordinates(lastClickRow+1, lastClickCol + 1, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow + 1, lastClickCol + 1] = 1;
                    buttonArray[lastClickRow + 1, lastClickCol + 1].BackgroundImage = Properties.Resources.dot6;
                }
            }
            else if (lastClickRow != 7 && lastClickCol != 7 && pieces[lastClickRow + 1, lastClickCol + 1] / 10 != p / 10)
            {
               // MessageBox.Show((lastClickRow + 1) + " " + (lastClickCol+1));
                if (RookPathOfKingForCoordinates(lastClickRow + 1, lastClickCol + 1) == false && BishopPathOfKingForCoordinates(lastClickRow + 1, lastClickCol + 1) == false && KnightPathOfKingForCoordinates(lastClickRow + 1, lastClickCol + 1, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow + 1, lastClickCol + 1] = 1;
                    buttonArray[lastClickRow + 1, lastClickCol + 1].BackColor = yellowColor;
                }
            }

            if (lastClickRow != 7 && lastClickCol != 0 && pieces[lastClickRow + 1, lastClickCol - 1] == 0)
            {
               // MessageBox.Show((lastClickRow + 1) + " " + (lastClickCol-1));
                if (RookPathOfKingForCoordinates(lastClickRow + 1, lastClickCol - 1) == false && BishopPathOfKingForCoordinates(lastClickRow + 1, lastClickCol - 1) == false && KnightPathOfKingForCoordinates(lastClickRow+1, lastClickCol - 1, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow + 1, lastClickCol - 1] = 1;
                    buttonArray[lastClickRow + 1, lastClickCol - 1].BackgroundImage = Properties.Resources.dot6;
                }
            }
            else if (lastClickRow != 7 && lastClickCol != 0 && pieces[lastClickRow + 1, lastClickCol - 1] / 10 != p / 10)
            {
                //MessageBox.Show((lastClickRow + 1) + " " + (lastClickCol-1));
                if (RookPathOfKingForCoordinates(lastClickRow + 1, lastClickCol - 1) == false && BishopPathOfKingForCoordinates(lastClickRow + 1, lastClickCol - 1) == false && KnightPathOfKingForCoordinates(lastClickRow+1, lastClickCol - 1, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow + 1, lastClickCol - 1] = 1;
                    buttonArray[lastClickRow + 1, lastClickCol - 1].BackColor = yellowColor;
                }
            }

            if (lastClickRow != 0 && lastClickCol != 7 && pieces[lastClickRow - 1, lastClickCol + 1] == 0)
            {
               // MessageBox.Show((lastClickRow - 1) + " " + (lastClickCol+1));
                if (RookPathOfKingForCoordinates(lastClickRow - 1, lastClickCol + 1) == false && BishopPathOfKingForCoordinates(lastClickRow - 1, lastClickCol + 1) == false && KnightPathOfKingForCoordinates(lastClickRow-1 , lastClickCol + 1, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow - 1, lastClickCol + 1] = 1;
                    buttonArray[lastClickRow - 1, lastClickCol + 1].BackgroundImage = Properties.Resources.dot6;
                }
            }
            else if (lastClickRow != 0 && lastClickCol != 7 && pieces[lastClickRow - 1, lastClickCol + 1] / 10 != p / 10)
            {
              //  MessageBox.Show((lastClickRow - 1) + " " + (lastClickCol+1));
                if (RookPathOfKingForCoordinates(lastClickRow - 1, lastClickCol + 1) == false && BishopPathOfKingForCoordinates(lastClickRow - 1, lastClickCol + 1) == false && KnightPathOfKingForCoordinates(lastClickRow-1, lastClickCol + 1, p) == false)
                {
                    coordinatesOfPossibleMoves[lastClickRow - 1, lastClickCol + 1] = 1;
                    buttonArray[lastClickRow - 1, lastClickCol + 1].BackColor = yellowColor;
                }
            }

        }

        public void KnightPath(int p)
        {
            if (checkFromBothSide) return;
            if ((isBishopCheck || isRookCheck) && isKnightCheck) return;
            int i = 0;
            int j = 0;
            if (!isRookCheck) i = RookPathOfKing();
            if (!isBishopCheck) j = BishopPathOfKing();
            if (i > 0 || j > 0) return;

            if (isRookCheck)
            {
                setOneBetweenKingAndRook();// isse waha bich me 1 rakh dega raja or jaha se check lgi hai uske bich
            }
            else if (isBishopCheck && isPawnCheck == false)
            {
                setOneBetweenKingAndBishop();// isse waha bich me 1 rakh dega raja or jaha se check lgi hai uske bich
            }


            int r = lastClickRow + 2;
            int c = lastClickCol + 1;
            if (r < 8 && c < 8)
            {
                if (pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10)
                {
                    
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r,c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackColor = yellowColor;
                    }
                }
                else if (pieces[r, c] == 0)
                {
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackgroundImage = Properties.Resources.dot6;
                    }
                }
            }
            c = lastClickCol - 1;
            if (r < 8 && c >= 0)
            {
                if (pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10)
                {
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackColor = yellowColor;
                    }
                }
                else if (pieces[r, c] == 0)
                {
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackgroundImage = Properties.Resources.dot6;
                    }
                }
            }
            r = lastClickRow - 2;
            if (r >= 0 && c >= 0)
            {
                if (pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10)
                {
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackColor = yellowColor;
                    }
                }
                else if (pieces[r, c] == 0)
                {
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackgroundImage = Properties.Resources.dot6;
                    }
                }
            }
            c = lastClickCol + 1;
            if (r >= 0 && c < 8)
            {
                if (pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10)
                {
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackColor = yellowColor;
                    }
                }
                else if (pieces[r, c] == 0)
                {
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackgroundImage = Properties.Resources.dot6;
                    }
                }
            }

            r = lastClickRow + 1;
            c = lastClickCol + 2;
            if (r < 8 && c < 8)
            {
                if (pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10)
                {
                    //MessageBox.Show(r+" <-r c-> "+c+" ccc"+checkCoordinates[r,c] );
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackColor = yellowColor;
                    }
                }
                else if (pieces[r, c] == 0)
                {

                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackgroundImage = Properties.Resources.dot6;
                    }
                }
            }
            c = lastClickCol - 2;
            if (r < 8 && c >= 0)
            {
                if (pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10)
                {
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackColor = yellowColor;
                    }
                }
                else if (pieces[r, c] == 0)
                {

                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackgroundImage = Properties.Resources.dot6;
                    }
                }
            }
            r = lastClickRow - 1;
            if (r >= 0 && c >= 0)
            {
                if (pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10)
                {
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackColor = yellowColor;
                    }
                }
                else if (pieces[r, c] == 0)
                {

                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackgroundImage = Properties.Resources.dot6;
                    }
                }
            }
            c = lastClickCol + 2;
            if (r >= 0 && c < 8)
            {
                if (pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10)
                {
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackColor = yellowColor;
                    }
                }
                else if (pieces[r, c] == 0)
                {
                    if ((isBishopCheck || isRookCheck || isKnightCheck) && checkCoordinates[r, c] == 0)
                    { }
                    else
                    {
                        coordinatesOfPossibleMoves[r, c] = 1;
                        buttonArray[r, c].BackgroundImage = Properties.Resources.dot6;
                    }
                }
            }

        }
   //*******************************************

        //***************************************************
        //Removing Path

        

        private void RemovePath()
        {
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(pieces[i, j] == 0 && coordinatesOfPossibleMoves[i, j] == 1)
                    {
                        coordinatesOfPossibleMoves[i, j] = 0;
                        buttonArray[i,j].BackgroundImage = null;
                        
                    }
                    else if(pieces[i,j]!=0 && coordinatesOfPossibleMoves[i, j] == 1)
                    {
                        coordinatesOfPossibleMoves[i, j] = 0;
                        char g = color[i, j];
                        if (g == 'b') buttonArray[i, j].BackColor = buttonColor1;
                        else buttonArray[i, j].BackColor = buttonColor2;
                    }
                }
            }
        }
        


        //************************************************

        //Rook Path Of King To find Check
        
        
        public int RookPathOfKing()
        {
            bool found=false;
            int r, c;
            int i;
            if (pieces[lastClickRow, lastClickCol] / 10 == 0 )//means piece is black
            {
                
                for(i = BlackKingRowIndex+1; i < 8; i++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (lastClickCol != BlackKingColumnIndex) break;
                    if(pieces[i,BlackKingColumnIndex]!=0 && pieces[i, BlackKingColumnIndex]/10 == 0 && lastClickRow!=i)// && lastClickCol!=BlackKingColumnIndex )
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if(pieces[i, BlackKingColumnIndex] != 0 && pieces[i, BlackKingColumnIndex]/10 != 0)
                    {
                        if (pieces[i, BlackKingColumnIndex] == 15 || pieces[i, BlackKingColumnIndex] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                    //MessageBox.Show("" + i);
                }
                if (found)
                {
                    return pieces[i, BlackKingColumnIndex];
                }
                for (i = BlackKingRowIndex - 1; i >= 0; i--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (lastClickCol != BlackKingColumnIndex) break; ;
                    if (pieces[i, BlackKingColumnIndex] != 0 && pieces[i, BlackKingColumnIndex] / 10 == 0 && lastClickRow != i )//&& lastClickCol != BlackKingColumnIndex)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[i, BlackKingColumnIndex] != 0 && pieces[i, BlackKingColumnIndex] / 10 != 0)
                    {
                        if (pieces[i, BlackKingColumnIndex] == 15 || pieces[i, BlackKingColumnIndex] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[i, BlackKingColumnIndex];
                }

                for (i = BlackKingColumnIndex + 1; i < 8; i++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (lastClickRow != BlackKingRowIndex) break; ;
                    if (pieces[BlackKingRowIndex, i] != 0 && pieces[BlackKingRowIndex, i] / 10 == 0 && lastClickCol != i)// && lastClickRow!=BlackKingRowIndex)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[BlackKingRowIndex, i] != 0 && pieces[BlackKingRowIndex, i] / 10 != 0)
                    {
                        if (pieces[BlackKingRowIndex, i] == 15 || pieces[BlackKingRowIndex, i] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[BlackKingRowIndex, i];
                }

                for (i = BlackKingColumnIndex - 1; i >= 0; i--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (lastClickRow != BlackKingRowIndex) break; ;
                    if (pieces[BlackKingRowIndex, i] != 0 && pieces[BlackKingRowIndex, i] / 10 == 0 && lastClickCol != i )//&& lastClickRow != BlackKingRowIndex)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[BlackKingRowIndex, i] != 0 && pieces[BlackKingRowIndex, i] / 10 != 0)
                    {
                        if (pieces[BlackKingRowIndex, i] == 15 || pieces[BlackKingRowIndex, i] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[BlackKingRowIndex, i];
                }


            }

            else//means piece is white
            {
                //MessageBox.Show("White");
                //Row Wise Check
                for (i = WhiteKingRowIndex + 1; i < 8; i++)
                {
                    //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (lastClickCol != WhiteKingColumnIndex) break; ;
                    if (pieces[i, WhiteKingColumnIndex] != 0 && pieces[i, WhiteKingColumnIndex] / 10 == 1 && lastClickRow != i )//&& lastClickCol != WhiteKingColumnIndex)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                                                                        // else if(pieces[i, BlackKingColumnIndex] != 0 && pieces[i, BlackKingColumnIndex]/10 != 0)
                    else if (pieces[i, WhiteKingColumnIndex] != 0 && pieces[i, WhiteKingColumnIndex] / 10 != 1)
                    {
                        if (pieces[i, WhiteKingColumnIndex] == 5 || pieces[i, WhiteKingColumnIndex] == 7)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[i, WhiteKingColumnIndex];
                }
                for (i = WhiteKingRowIndex - 1; i >= 0; i--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (lastClickCol != WhiteKingColumnIndex) break; ;
                    if (pieces[i, WhiteKingColumnIndex] != 0 && pieces[i, WhiteKingColumnIndex] / 10 == 1 && lastClickRow != i )//&& lastClickCol != WhiteKingColumnIndex)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[i, WhiteKingColumnIndex] != 0 && pieces[i, WhiteKingColumnIndex] / 10 != 1)
                    {
                        if (pieces[i, WhiteKingColumnIndex] == 5 || pieces[i, WhiteKingColumnIndex] == 7)
                        {
                            //MessageBox.Show("Yaha Pr");
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[i, WhiteKingColumnIndex];
                }

                //Column Wise Check
                for (i = WhiteKingColumnIndex + 1; i < 8; i++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (lastClickRow != WhiteKingRowIndex) break; ;
                    if (pieces[WhiteKingRowIndex, i] != 0 && pieces[WhiteKingRowIndex, i] / 10 == 1 && lastClickCol != i )//&& lastClickRow != WhiteKingRowIndex)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[WhiteKingRowIndex, i] != 0 && pieces[WhiteKingRowIndex, i] / 10 != 1)
                    {
                        if (pieces[WhiteKingRowIndex, i] == 5 || pieces[WhiteKingRowIndex, i] == 7)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[WhiteKingRowIndex, i];
                }

                for (i = WhiteKingColumnIndex - 1; i >= 0; i--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (lastClickRow != WhiteKingRowIndex) break; ;
                    if (pieces[WhiteKingRowIndex, i] != 0 && pieces[WhiteKingRowIndex, i] / 10 == 1 && lastClickCol != i )//&& lastClickRow != WhiteKingRowIndex)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[WhiteKingRowIndex, i] != 0 && pieces[WhiteKingRowIndex, i] / 10 != 1)
                    {
                        if (pieces[WhiteKingRowIndex, i] == 5 || pieces[WhiteKingRowIndex, i] == 7)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[WhiteKingRowIndex, i];
                }


            }
            return 0;
        }


        //Bishop Path Of King
        public int BishopPathOfKing()
        {
            bool found = false;
            int r, c;
            int i,j;
            if (pieces[lastClickRow, lastClickCol] / 10 == 0)//means piece is black jo click hua hai
            {
                i = BlackKingRowIndex + 1;
                j = BlackKingColumnIndex + 1;
                
                for (; i < 8 && j < 8; i++,j++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h

                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 0)
                    {
                        if (lastClickCol == j && lastClickRow == i) continue;
                        break;
                    }                                                //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 0)
                    {
                        if (pieces[i, j] == 18 || pieces[i, j] == 17 )
                        {
                            found = true;
                        }
                        break;
                    }
                    //MessageBox.Show("" + i);
                }
                if (found)
                {
                    return pieces[i, j];
                }
                for (i = BlackKingRowIndex - 1,j = BlackKingColumnIndex-1; i >= 0 && j>=0; i--,j--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 0)
                    {
                        if (lastClickCol == j && lastClickRow == i) continue;
                        break;
                    }                                                 //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 0)
                    {
                        if (pieces[i, j] == 18 || pieces[i, j] == 17 )
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[i, j];
                }

                for (i = BlackKingRowIndex-1,j = BlackKingColumnIndex + 1; i>=0 && j<8; i--,j++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 0)
                    {
                        if (lastClickCol == j && lastClickRow == i) continue;
                        break;
                    }                                                  //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 0)
                    {
                        if (pieces[i,j] == 18 || pieces[i,j] == 17 )
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[i, j];
                }

                for (i = BlackKingRowIndex + 1,j = BlackKingColumnIndex-1; i<8 && j >= 0; i++,j--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 0)
                    {
                        if (lastClickCol == j && lastClickRow == i) continue;
                        break;
                    }                                                 //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 0)
                    {
                        ///********************yaha change kiya h mene
                        if (pieces[i, j] == 18 || pieces[i, j] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[i, j];
                }


            }

            else//means piece is white
            {
                //MessageBox.Show("White");
                //Row Wise Check
                for (i = WhiteKingRowIndex + 1, j = WhiteKingColumnIndex + 1; i < 8 && j < 8; i++, j++)
                {           //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h

                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1)
                    {
                        if (lastClickCol == j && lastClickRow == i) continue;
                        break;
                    }                                                 //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 1)
                    {
                        if (pieces[i, j] == 8 || pieces[i, j] == 7)
                        {
                            //MessageBox.Show("If ka 1st loop else if");
                            found = true;
                        }
                        break;
                    }
                    //MessageBox.Show("" + i);
                }
                if (found)
                {
                    return pieces[i, j];
                }
                for (i = WhiteKingRowIndex - 1, j = WhiteKingColumnIndex - 1; i >= 0 && j >= 0; i--, j--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1)
                    {
                        if (lastClickCol == j && lastClickRow == i) continue;
                        break;
                    }                                                 //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 1)
                    {
                        if (pieces[i, j] == 8 || pieces[i, j] == 7)
                        {
                            //MessageBox.Show("If ka 2nd loop else if");
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[i, j];
                }

                for (i = WhiteKingRowIndex - 1, j = WhiteKingColumnIndex + 1; i >= 0 && j < 8; i--, j++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    //if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1 && lastClickCol != j && lastClickRow != i)
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1 )
                    {
                        if (lastClickCol == j && lastClickRow == i) continue;
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 1)
                    {
                        if (pieces[i,j] == 8 || pieces[i,j] == 7)
                        {
                            //MessageBox.Show("If ka 3rd loop else if");
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[i, j];
                }

                for (i = WhiteKingRowIndex + 1, j = WhiteKingColumnIndex - 1; i < 8 && j >= 0; i++, j--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1)
                    {
                        if (lastClickCol == j && lastClickRow == i) continue;
                        break;
                    }                                                  //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 1)
                    {
                        if (pieces[i, j] == 8 || pieces[i, j] == 7)
                        {
                            //MessageBox.Show("If ka 4th loop else if");
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    return pieces[i, j];
                }


            }
            return 0;
        }

        //************************************************  


        public int CheckRookPathOfKingForCheck()
        {
            bool found = false;
            int r, c;
            int i;
            if (!whiteChance)//means piece is black
            {

                for (i = BlackKingRowIndex + 1; i < 8; i++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h

                    if (pieces[i, BlackKingColumnIndex] != 0 && pieces[i, BlackKingColumnIndex] / 10 == 0 )
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[i, BlackKingColumnIndex] != 0 && pieces[i, BlackKingColumnIndex] / 10 != 0)
                    {
                        if (pieces[i, BlackKingColumnIndex] == 15 || pieces[i, BlackKingColumnIndex] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                    //MessageBox.Show("" + i);
                }
                if (found)
                {
                    isRookCheck= true;
                    RookRowIndex = i;
                    RookColIndex = BlackKingColumnIndex;
                    return pieces[i, BlackKingColumnIndex];
                }
                for (i = BlackKingRowIndex - 1; i >= 0; i--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, BlackKingColumnIndex] != 0 && pieces[i, BlackKingColumnIndex] / 10 == 0 )
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[i, BlackKingColumnIndex] != 0 && pieces[i, BlackKingColumnIndex] / 10 != 0)
                    {
                        if (pieces[i, BlackKingColumnIndex] == 15 || pieces[i, BlackKingColumnIndex] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isRookCheck = true;
                    RookRowIndex = i;
                    RookColIndex = BlackKingColumnIndex;
                    return pieces[i, BlackKingColumnIndex];
                }

                for (i = BlackKingColumnIndex + 1; i < 8; i++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[BlackKingRowIndex, i] != 0 && pieces[BlackKingRowIndex, i] / 10 == 0 )
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[BlackKingRowIndex, i] != 0 && pieces[BlackKingRowIndex, i] / 10 != 0)
                    {
                        if (pieces[BlackKingRowIndex, i] == 15 || pieces[BlackKingRowIndex, i] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isRookCheck = true;
                    RookRowIndex = BlackKingRowIndex;
                    RookColIndex = i;
                    return pieces[BlackKingRowIndex, i];
                }

                for (i = BlackKingColumnIndex - 1; i >= 0; i--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[BlackKingRowIndex, i] != 0 && pieces[BlackKingRowIndex, i] / 10 == 0 )
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[BlackKingRowIndex, i] != 0 && pieces[BlackKingRowIndex, i] / 10 != 0)
                    {
                        if (pieces[BlackKingRowIndex, i] == 15 || pieces[BlackKingRowIndex, i] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isRookCheck = true;
                    RookRowIndex = BlackKingRowIndex;
                    RookColIndex = i;
                    return pieces[BlackKingRowIndex, i];
                }


            }

            else//means piece is white
            {
                //MessageBox.Show("White");
                //Row Wise Check
                for (i = WhiteKingRowIndex + 1; i < 8; i++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, WhiteKingColumnIndex] != 0 && pieces[i, WhiteKingColumnIndex] / 10 == 1 )//&& lastClickCol != WhiteKingColumnIndex)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                                                                        // else if(pieces[i, BlackKingColumnIndex] != 0 && pieces[i, BlackKingColumnIndex]/10 != 0)
                    else if (pieces[i, WhiteKingColumnIndex] != 0 && pieces[i, WhiteKingColumnIndex] / 10 != 1)
                    {
                        if (pieces[i, WhiteKingColumnIndex] == 5 || pieces[i, WhiteKingColumnIndex] == 7)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isRookCheck = true;
                    RookRowIndex = i;
                    RookColIndex = WhiteKingColumnIndex;
                    return pieces[i, WhiteKingColumnIndex];
                }
                for (i = WhiteKingRowIndex - 1; i >= 0; i--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, WhiteKingColumnIndex] != 0 && pieces[i, WhiteKingColumnIndex] / 10 == 1 )// && lastClickCol != WhiteKingColumnIndex)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[i, WhiteKingColumnIndex] != 0 && pieces[i, WhiteKingColumnIndex] / 10 != 1)
                    {
                        if (pieces[i, WhiteKingColumnIndex] == 5 || pieces[i, WhiteKingColumnIndex] == 7)
                        {
                            //MessageBox.Show("Yaha Pr");
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isRookCheck = true;
                    RookRowIndex = i;
                    RookColIndex = WhiteKingColumnIndex;
                    return pieces[i, WhiteKingColumnIndex];
                }

                //Column Wise Check
                for (i = WhiteKingColumnIndex + 1; i < 8; i++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[WhiteKingRowIndex, i] != 0 && pieces[WhiteKingRowIndex, i] / 10 == 1 )
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[WhiteKingRowIndex, i] != 0 && pieces[WhiteKingRowIndex, i] / 10 != 1)
                    {
                        if (pieces[WhiteKingRowIndex, i] == 5 || pieces[WhiteKingRowIndex, i] == 7)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isRookCheck = true;
                    RookRowIndex = WhiteKingRowIndex;
                    RookColIndex = i;
                    return pieces[WhiteKingRowIndex, i];
                }

                for (i = WhiteKingColumnIndex - 1; i >= 0; i--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[WhiteKingRowIndex, i] != 0 && pieces[WhiteKingRowIndex, i] / 10 == 1)
                    {
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[WhiteKingRowIndex, i] != 0 && pieces[WhiteKingRowIndex, i] / 10 != 1)
                    {
                        if (pieces[WhiteKingRowIndex, i] == 5 || pieces[WhiteKingRowIndex, i] == 7)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isRookCheck = true;
                    RookRowIndex = WhiteKingRowIndex;
                    RookColIndex = i;
                    return pieces[WhiteKingRowIndex, i];
                }


            }
            return 0;
        }


        //Bishop Path Of King
        public int CheckBishopPathOfKingForCheck()
        {
            bool found = false;
            int r, c;
            int i, j;
            if (!whiteChance)//means piece is black jo click hua hai
            {
                i = BlackKingRowIndex + 1;
                j = BlackKingColumnIndex + 1;
                if (i < 8 && j < 8 && pieces[i, j] == 11) //yaha per check kiya hai ki pawn check hai kya
                {
                    isPawnCheck = true;
                    isBishopCheck = true;
                    checkCoordinates[i, j] = 1;
                    BishopRowIndex = i;
                    BishopColIndex = j;
                    return 11;
                }
                j = BlackKingColumnIndex - 1;
                if (i < 8 && j >= 0 && pieces[i, j] == 11) // yaha per check kiya hai ki pawn check hai 
                {
                    isPawnCheck = true;
                    isBishopCheck = true;
                    checkCoordinates[i, j] = 1;
                    BishopRowIndex = i;
                    BishopColIndex = j;
                    return 11;
                }
                j = BlackKingColumnIndex + 1;
                for (; i < 8 && j < 8; i++, j++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h

                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 0)
                    {
                        
                        break;
                    }                                                //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 0)
                    {
                        if (pieces[i, j] == 18 || pieces[i, j] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                    //MessageBox.Show("" + i);
                }
                if (found)
                {
                    isBishopCheck = true;
                    BishopRowIndex = i;
                    BishopColIndex = j;
                    return pieces[i, j];
                }
                for (i = BlackKingRowIndex - 1, j = BlackKingColumnIndex - 1; i >= 0 && j >= 0; i--, j--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 0)
                    {
                        
                        break;
                    }                                                 //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 0)
                    {
                        if (pieces[i, j] == 18 || pieces[i, j] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isBishopCheck = true;
                    BishopRowIndex = i;
                    BishopColIndex = j;
                    return pieces[i, j];
                }

                for (i = BlackKingRowIndex - 1, j = BlackKingColumnIndex + 1; i >= 0 && j < 8; i--, j++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 0)
                    {
                        
                        break;
                    }                                                  //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 0)
                    {
                        if (pieces[i, j] == 18 || pieces[i, j] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isBishopCheck = true;
                    BishopRowIndex = i;
                    BishopColIndex = j;
                    return pieces[i, j];
                }

                for (i = BlackKingRowIndex + 1, j = BlackKingColumnIndex - 1; i < 8 && j >= 0; i++, j--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 0)
                    {
                        
                        break;
                    }                                                 //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 0)
                    {
                        if (pieces[i, j] == 18 || pieces[i, j] == 17)
                        {
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isBishopCheck = true;
                    BishopRowIndex = i;
                    BishopColIndex = j;
                    return pieces[i, j];
                }


            }

            else//means piece is white
            {
                //MessageBox.Show("White");
                //Row Wise Check
                i = WhiteKingRowIndex - 1;
                j = WhiteKingColumnIndex - 1;
                if (i >= 0 && j >= 0 && pieces[i, j] == 1) // ise dekhega ki pawn check hai kya
                {
                    isPawnCheck = true;
                    isBishopCheck=true;
                    checkCoordinates[i, j] = 1;
                    BishopRowIndex = i;
                    BishopColIndex = j;
                    return 1;

                }
                i = WhiteKingRowIndex - 1;
                j = WhiteKingColumnIndex + 1;
                if (i >= 0 && j < 8 && pieces[i, j] == 1) // ise dekhega ki pawn check hai kya
                {
                    isPawnCheck = true;
                    isBishopCheck = true;
                    checkCoordinates[i, j] = 1;
                    BishopRowIndex = i;
                    BishopColIndex = j;
                    return 1;

                }
                for (i = WhiteKingRowIndex + 1, j = WhiteKingColumnIndex + 1; i < 8 && j < 8; i++, j++)
                {           //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h

                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1)
                    {
                        
                        break;
                    }                                                 //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 1)
                    {
                        if (pieces[i, j] == 8 || pieces[i, j] == 7)
                        {
                            //MessageBox.Show("If ka 1st loop else if");
                            found = true;
                        }
                        break;
                    }
                    //MessageBox.Show("" + i);
                }
                if (found)
                {
                    isBishopCheck = true;
                    BishopRowIndex = i;
                    BishopColIndex = j;
                    return pieces[i, j];
                }

                for (i = WhiteKingRowIndex - 1, j = WhiteKingColumnIndex - 1; i >= 0 && j >= 0; i--, j--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1)
                    {
                        
                        break;
                    }                                                 //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 1)
                    {
                        if (pieces[i, j] == 8 || pieces[i, j] == 7)
                        {
                            //MessageBox.Show("If ka 2nd loop else if");
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isBishopCheck = true;
                    BishopRowIndex = i;
                    BishopColIndex = j;
                    return pieces[i, j];
                }

                for (i = WhiteKingRowIndex - 1, j = WhiteKingColumnIndex + 1; i >= 0 && j < 8; i--, j++)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    //if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1 && lastClickCol != j && lastClickRow != i)
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1)
                    {
                        
                        break;
                    }                                                   //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 1)
                    {
                        if (pieces[i, j] == 8 || pieces[i, j] == 7)
                        {
                            //MessageBox.Show("If ka 3rd loop else if");
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isBishopCheck = true;
                    BishopRowIndex = i;
                    BishopColIndex = j;
                    return pieces[i, j];
                }

                for (i = WhiteKingRowIndex + 1, j = WhiteKingColumnIndex - 1; i < 8 && j >= 0; i++, j--)
                {                                               //khud ka piece hai                         mtlb vo whi piece hai jispar click kiya h
                    if (pieces[i, j] != 0 && pieces[i, j] / 10 == 1)
                    {
                       
                        break;
                    }                                                  //piece opposite site ka h
                    else if (pieces[i, j] != 0 && pieces[i, j] / 10 != 1)
                    {
                        if (pieces[i, j] == 8 || pieces[i, j] == 7)
                        {
                            //MessageBox.Show("If ka 4th loop else if");
                            found = true;
                        }
                        break;
                    }
                }
                if (found)
                {
                    isBishopCheck = true;
                    BishopRowIndex = i;
                    BishopColIndex = j;
                    return pieces[i, j];
                }


            }
            return 0;
        }

        //knight path of king
                                                 //p is for piece number
        public int CheckKnightPathOfKingForCheck() // ye function check krega ki knight se raja ko check lag rhi hai kya
        {
            count = 0;
            int p = pieces[row, col];
            int r = row + 2;
            int c = col + 1;
            if (r < 8 && c < 8)
            {                               
                if ((p==9 || p==19) &&pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10 && (pieces[r,c]==16 || pieces[r,c]==6))
                {
                    checkCoordinates[row, col] = 1;
                    possibleRowIndex[count] = row;
                    possibleColIndex[count] = col;
                    count++;
                    isKnightCheck = true;
                    return p;
                }
                
            }
            c = col - 1;
            if (r < 8 && c >= 0)
            {
                if ((p == 9 || p == 19) && pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10 && (pieces[r, c] == 16 || pieces[r, c] == 6))
                {
                    possibleRowIndex[count] = row;
                    possibleColIndex[count] = col;
                    count++;
                    checkCoordinates[row, col] = 1;
                    isKnightCheck = true;
                    return p;
                }
            }
            r = row - 2;
            if (r >= 0 && c >= 0)
            {
                if ((p == 9 || p == 19) && pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10 && (pieces[r, c] == 16 || pieces[r, c] == 6))
                {
                    possibleRowIndex[count] = row;
                    possibleColIndex[count] = col;
                    count++;
                    checkCoordinates[row, col] = 1;
                    isKnightCheck = true;
                    return p;
                }
            }
            c = col + 1;
            if (r >= 0 && c < 8)
            {
                if ((p == 9 || p == 19) && pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10 && (pieces[r, c] == 16 || pieces[r, c] == 6))
                {
                    possibleRowIndex[count] = row;
                    possibleColIndex[count] = col;
                    count++; 
                    checkCoordinates[row, col] = 1;
                    isKnightCheck = true;
                    return p;
                }
            }

            r =row + 1;
            c = col+ 2;
            if (r < 8 && c < 8)
            {
                if ((p == 9 || p == 19) && pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10 && (pieces[r, c] == 16 || pieces[r, c] == 6))
                {
                    possibleRowIndex[count] = row;
                    possibleColIndex[count] = col;

                    count++;
                    checkCoordinates[row, col] = 1;
                    isKnightCheck = true;
                    return p;
                }
            }
            c = col- 2;
            if (r < 8 && c >= 0)
            {
                if ((p == 9 || p == 19) && pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10 && (pieces[r, c] == 16 || pieces[r, c] == 6))
                {
                    possibleRowIndex[count] = row;
                    possibleColIndex[count] = col;
                    count++; 
                    checkCoordinates[row, col] = 1;
                    isKnightCheck = true;
                    return p;
                }
            }
            r = row- 1;
            if (r >= 0 && c >= 0)
            {
                if ((p == 9 || p == 19) && pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10 && (pieces[r, c] == 16 || pieces[r, c] == 6))
                {
                    possibleRowIndex[count] = row;
                    possibleColIndex[count] = col;
                    count++; 
                    checkCoordinates[row, col] = 1;
                    isKnightCheck = true;
                    return p;
                }
            }
            c = col+ 2;
            if (r >= 0 && c < 8)
            {
                if ((p == 9 || p == 19) && pieces[r, c] != 0 && pieces[r, c] / 10 != p / 10 && (pieces[r, c] == 16 || pieces[r, c] == 6))
                {
                    possibleRowIndex[count] = row;
                    possibleColIndex[count] = col;
                    count++; 
                    checkCoordinates[row, col] = 1;
                    isKnightCheck = true;
                    return p;
                }
            }




            return 0;
        }
        //**********************************************
        public void RestrictedPathOfRookAndQueen(int i)
        {
            
                if (pieces[lastClickRow, lastClickCol] / 10 == 1) //white piece
                {
                    if (lastClickCol == WhiteKingColumnIndex)// iska mtlb row wise chlna hai
                    {
                        int j;
                        for (j = lastClickRow + 1; pieces[j, lastClickCol] != i && pieces[j, lastClickCol] != 16; j++)
                        {
                            coordinatesOfPossibleMoves[j, lastClickCol] = 1;
                            buttonArray[j, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[j, lastClickCol] == i)
                        {
                            coordinatesOfPossibleMoves[j, lastClickCol] = 1;
                            buttonArray[j, lastClickCol].BackColor = yellowColor;
                        }
                        for (j = lastClickRow - 1; pieces[j, lastClickCol] != i && pieces[j, lastClickCol] != 16; j--)
                        {
                            coordinatesOfPossibleMoves[j, lastClickCol] = 1;
                            buttonArray[j, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[j, lastClickCol] == i)
                        {
                            coordinatesOfPossibleMoves[j, lastClickCol] = 1;
                            buttonArray[j, lastClickCol].BackColor = yellowColor;
                        }

                    }
                    else// column wise chlna  hai
                    {
                        int j;
                        for (j = lastClickCol + 1; pieces[lastClickRow, j] != i && pieces[lastClickRow, j] != 16; j++)
                        {
                            coordinatesOfPossibleMoves[lastClickRow, j] = 1;
                            buttonArray[lastClickRow, j].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[lastClickRow, j] == i)
                        {
                            coordinatesOfPossibleMoves[lastClickRow, j] = 1;
                            buttonArray[lastClickRow, j].BackColor = yellowColor;
                        }
                        for (j = lastClickCol - 1; pieces[lastClickRow, j] != i && pieces[lastClickRow, j] != 16; j--)
                        {
                            coordinatesOfPossibleMoves[lastClickRow, j] = 1;
                            buttonArray[lastClickRow, j].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[lastClickRow, j] == i)
                        {
                            coordinatesOfPossibleMoves[lastClickRow, j] = 1;
                            buttonArray[lastClickRow, j].BackColor = yellowColor;
                        }
                    }
                }
                else //black piece
                {

                    if (lastClickCol == BlackKingColumnIndex)// iska mtlb row wise chlna hai
                    {
                        int j;
                        for (j = lastClickRow + 1; pieces[j, lastClickCol] != i && pieces[j, lastClickCol] != 6; j++)
                        {
                            coordinatesOfPossibleMoves[j, lastClickCol] = 1;
                            buttonArray[j, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[j, lastClickCol] == i)
                        {
                            coordinatesOfPossibleMoves[j, lastClickCol] = 1;
                            buttonArray[j, lastClickCol].BackColor = yellowColor;
                        }
                        for (j = lastClickRow - 1; pieces[j, lastClickCol] != i && pieces[j, lastClickCol] != 6; j--)
                        {
                            coordinatesOfPossibleMoves[j, lastClickCol] = 1;
                            buttonArray[j, lastClickCol].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[j, lastClickCol] == i)
                        {
                            coordinatesOfPossibleMoves[j, lastClickCol] = 1;
                            buttonArray[j, lastClickCol].BackColor = yellowColor;
                        }

                    }
                    else// column wise chlna  hai
                    {
                        int j;
                        for (j = lastClickCol + 1; pieces[lastClickRow, j] != i && pieces[lastClickRow, j] != 6; j++)
                        {
                            coordinatesOfPossibleMoves[lastClickRow, j] = 1;
                            buttonArray[lastClickRow, j].BackgroundImage = Properties.Resources.dot6;
                        }
                        if ( pieces[lastClickRow, j] == i)
                        {
                            coordinatesOfPossibleMoves[lastClickRow, j] = 1;
                            buttonArray[lastClickRow, j].BackColor = yellowColor;
                        }
                        for (j = lastClickCol - 1; pieces[lastClickRow, j] != i && pieces[lastClickRow, j] != 6; j--)
                        {
                            coordinatesOfPossibleMoves[lastClickRow, j] = 1;
                            buttonArray[lastClickRow, j].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[lastClickRow, j] == i)
                        {
                            coordinatesOfPossibleMoves[lastClickRow, j] = 1;
                            buttonArray[lastClickRow, j].BackColor = yellowColor;
                        }
                    }
                }
            
        }

        public void RestrictedPathOfBishopAndQueen(int i)
        {
          
                int x, y;
                if (pieces[lastClickRow, lastClickCol] / 10 == 1) //white piece
                {
                    if (lastClickCol < WhiteKingColumnIndex && lastClickRow < WhiteKingRowIndex) // mtlb left side hai
                    {
                    x = lastClickRow - 1; y = lastClickCol - 1;

                        for (; x >= 0 && y >= 0 && pieces[x, y] != i && pieces[x, y] != 16 /*&& pieces[x, y] / 10 != 1*/; x--, y--)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[x, y] == i)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackColor = yellowColor;
                        }
                    x = lastClickRow + 1; y = lastClickCol + 1;
                        for (; x < 8 && y < 8 && pieces[x, y] != i && pieces[x, y] != 16 /*&& pieces[x, y] / 10 != 1*/; x++, y++)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[x, y] == i)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackColor = yellowColor;
                        }




                    }
                    else//mtlb right side hai
                    {
                    x = lastClickRow - 1; y = lastClickCol + 1;
                        for (; x >= 0 && y < 8 && pieces[x, y] != i && pieces[x, y] != 16 /*&& pieces[x, y] / 10 != 1*/; x--, y++)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[x, y] == i)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackColor = yellowColor;
                        }
                    x = lastClickRow + 1; y = lastClickCol - 1;
                        for (; x < 8 && y >= 0 && pieces[x, y] != i && pieces[x, y] != 16/* && pieces[x, y] / 10 != 1*/; x++, y--)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[x, y] == i)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackColor = yellowColor;
                        }



                    }





                }
                else //black piece
                {
                    if (lastClickCol > BlackKingColumnIndex && lastClickRow > BlackKingRowIndex) // mtlb left side hai
                    {
                    x = lastClickRow - 1;
                    y = lastClickCol - 1;
                        for (; x >= 0 && y >= 0 && pieces[x, y] != i && pieces[x, y] != 6 /*&& pieces[x, y] / 10 != 0*/; x--, y--)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[x, y] == i)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackColor = yellowColor;
                        }
                    x = lastClickRow + 1;
                    y = lastClickCol + 1;
                        for (; x < 8 && y < 8 && pieces[x, y] != i && pieces[x, y] != 6 /*&& pieces[x, y] / 10 != 0*/; x++, y++)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (x <8 && y<8 && pieces[x, y] == i)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackColor = yellowColor;
                        }




                    }
                    else//mtlb right side hai
                    {
                    x = lastClickRow - 1;y = lastClickCol + 1;
                        for (; x >= 0 && y < 8 && pieces[x, y] != i && pieces[x, y] != 6 /*&& pieces[x,y]/10!=0 ;*/; x--, y++)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (x >= 0 && y < 8 && pieces[x, y] == i)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackColor = yellowColor;
                        }
                    x = lastClickRow + 1; y = lastClickCol - 1;
                        for (; x < 8 && y >= 0 && pieces[x, y] != i && pieces[x, y] != 6/* && pieces[x, y] / 10 != 0*/; x++, y--)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackgroundImage = Properties.Resources.dot6;
                        }
                        if (pieces[x, y] == i)
                        {
                            coordinatesOfPossibleMoves[x, y] = 1;
                            buttonArray[x, y].BackColor = yellowColor;
                        }



                    }



                }
           
        }

        //************************
        private void PossibleMoves(Button b)
        {
            if (pieces[lastClickRow, lastClickCol] != 0)
            {
                int pieceNumber = pieces[lastClickRow, lastClickCol];
                //Yeh kale k piece ki bat hai
                if (pieceNumber == 5 || pieceNumber==15) RookPath(pieceNumber);
                else if(pieceNumber == 11 || pieceNumber == 1) PawnPath(pieceNumber);    
                else if(pieceNumber==8 || pieceNumber == 18) BishopPath(pieceNumber);
                else if(pieceNumber==7 || pieceNumber == 17) QueenPath(pieceNumber);
                else if(pieceNumber==6 || pieceNumber == 16) KingPath(pieceNumber);
                else if(pieceNumber==9 || pieceNumber == 19) KnightPath(pieceNumber);
            }
        }



        private void RemovePreviousPossibleMoves()
        {
            if (pieces[lastClickRow, lastClickCol] != 0)
            {
                //int pieceNumber = pieces[lastClickRow, lastClickCol];
                //Yeh kale k piece ki bat hai
                RemovePath();
            }
        }
        bool isIncrement = false;

    

        private bool IsMovementPossible(Button b)
        {

            int i = 0;
            if (coordinatesOfPossibleMoves[row, col] == 1)
            {
                if (isLeftBlackRookMoved == false && lastClickRow == 0 && lastClickCol == 0)
                {
                    i = 1;
                    isLeftBlackRookMoved = true;
                }
                if (isRightBlackRookMoved == false && lastClickRow == 0 && lastClickCol == 7)
                {
                    i = 1;
                    isRightBlackRookMoved = true;
                }
                if (isLeftWhiteRookMoved == false && lastClickRow == 7 && lastClickCol == 0)
                {
                    i = 1;
                    isLeftWhiteRookMoved = true;
                }
                if (isRightWhiteRookMoved == false && lastClickRow == 7 && lastClickCol == 7)
                {
                    i = 1;
                    isRightWhiteRookMoved = true;
                }
                if (isWhiteKingMoved == false && lastClickRow == 7 && lastClickCol == 4)
                {  
                    if (row == 7 && col == 2)
                    {
                        pieces[7, 0] = 0;
                        pieces[7, 3] = 15;

                        buttonArray[7, 3].BackgroundImage = buttonArray[7, 0].BackgroundImage;
                        buttonArray[7, 0].BackgroundImage = null;
                        isLeftWhiteRookMoved = true;
                    }
                    else if (row == 7 && col == 6)
                    {
                        pieces[7, 7] = 0;
                        pieces[7, 5] = 15;

                        buttonArray[7, 5].BackgroundImage = buttonArray[7, 0].BackgroundImage;
                        buttonArray[7, 7].BackgroundImage = null;
                        isRightWhiteRookMoved = true;
                    }
                    isWhiteKingMoved = true;
                    
                    
                }
                if (isBlackKingMoved == false && lastClickRow == 0 && lastClickCol == 4)
                {
                    if (row == 0 && col == 2)
                    {
                        pieces[0, 0] = 0;
                        pieces[0, 3] = 5;

                        buttonArray[0, 3].BackgroundImage = buttonArray[0, 0].BackgroundImage;
                        buttonArray[0, 0].BackgroundImage = null;
                        isLeftBlackRookMoved = true;

                    }
                    else if (row == 0 && col == 6)
                    {
                        pieces[0, 7] = 0;
                        pieces[0, 5] = 5;

                        buttonArray[0, 5].BackgroundImage = buttonArray[0, 0].BackgroundImage;
                        buttonArray[0, 7].BackgroundImage = null;
                        isRightBlackRookMoved = true;
                    }
                    
                    
                   
                    isBlackKingMoved = true;
                }
                if (whiteChance)
                {
                    if (pieces[lastClickRow, lastClickCol] == 16)
                    {
                        whiteKingMoveCount++;
                    }   
                    if(pieces[row, col] != 0)
                    {

                        blackKilledPiecesNumber[blackKilledCount] = pieces[row, col];
                        panel4.Visible =true;
                        blackPictureBox[blackKilledCount].SizeMode = PictureBoxSizeMode.StretchImage;
                        blackPictureBox[blackKilledCount].Image = buttonArray[row, col].BackgroundImage;
                        blackKilledCount++;

                    
                    }
                    if (pieces[lastClickRow, lastClickCol] == 11 && row == 0)
                    {
                        isIncrement = true;
                        panel2.Visible = true;

                        QueenPictureBox.Image = Properties.Resources.whitequeen;
                        QueenPictureBox.Visible = true;
                        QueenPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        RookPictureBox.Image = Properties.Resources.whiterook;
                        RookPictureBox.Visible = true;
                        RookPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        KnightPictureBox.Visible = true;
                        KnightPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        KnightPictureBox.Image = Properties.Resources.whiteknight;

                        BishopPictureBox.Image = Properties.Resources.whitebishop1;
                        BishopPictureBox.Visible = true;
                        BishopPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                else
                {
                    if (pieces[lastClickRow, lastClickCol] == 6)
                    {
                        blackKingMoveCount++;
                    }
                    if (pieces[row, col] != 0)
                    {
                        whiteKilledPiecesNumber[whiteKilledCount]=pieces[row, col];
                        panel3.Visible = true;
                        whitePictureBox[whiteKilledCount].SizeMode = PictureBoxSizeMode.StretchImage;
                        whitePictureBox[whiteKilledCount].Image = buttonArray[row, col].BackgroundImage;
                        whiteKilledCount++;


                    }
                    if (pieces[lastClickRow, lastClickCol] == 1 && row == 7)
                    {
                        isIncrement = true;
                        panel2.Visible = true;

                        QueenPictureBox.Image = Properties.Resources.blackqueen;
                        QueenPictureBox.Visible = true;
                        QueenPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        RookPictureBox.Image = Properties.Resources.blackrook;
                        RookPictureBox.Visible = true;
                        RookPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        KnightPictureBox.Visible = true;
                        KnightPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        KnightPictureBox.Image = Properties.Resources.blackknight;

                        BishopPictureBox.Image = Properties.Resources.blackbishop2;
                        BishopPictureBox.Visible = true;
                        BishopPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            if (whiteKilledCount == 15 && blackKilledCount == 15)
            {
                MessageBox.Show("Draw");
                panel1.Enabled = false;
            }
            PiecesData pd;
            if (pieces[lastClickRow, lastClickCol] == 15 && isLeftWhiteRookMoved == true && i == 1)
            {
                pd = new PiecesData(lastClickRow, lastClickCol, row, col, pieces[row, col], pieces[lastClickRow, lastClickCol], buttonArray[row, col].BackgroundImage, buttonArray[lastClickRow, lastClickCol].BackgroundImage, false, true, false, false);
            }
            else if (pieces[lastClickRow, lastClickCol] == 15 && isRightWhiteRookMoved == true && i == 1)
            {
                pd = new PiecesData(lastClickRow, lastClickCol, row, col, pieces[row, col], pieces[lastClickRow, lastClickCol], buttonArray[row, col].BackgroundImage, buttonArray[lastClickRow, lastClickCol].BackgroundImage, true, false, false, false);
            }
            else if (pieces[lastClickRow, lastClickCol] == 5 && isLeftBlackRookMoved == true && i == 1)
            {
                pd = new PiecesData(lastClickRow, lastClickCol, row, col, pieces[row, col], pieces[lastClickRow, lastClickCol], buttonArray[row, col].BackgroundImage, buttonArray[lastClickRow, lastClickCol].BackgroundImage, false, false, false, true);
            }
            else if (pieces[lastClickRow, lastClickCol] == 5 && isRightBlackRookMoved == true && i == 1)
            {
                pd = new PiecesData(lastClickRow, lastClickCol, row, col, pieces[row, col], pieces[lastClickRow, lastClickCol], buttonArray[row, col].BackgroundImage, buttonArray[lastClickRow, lastClickCol].BackgroundImage, false, false, true, false);
            }
            else
            {
                pd = new PiecesData(lastClickRow, lastClickCol, row, col, pieces[row, col], pieces[lastClickRow, lastClickCol], buttonArray[row, col].BackgroundImage, buttonArray[lastClickRow, lastClickCol].BackgroundImage,false,false,false,false);
            }
            stack.Push(pd);
            button2.Enabled = true;

            if (coordinatesOfPossibleMoves[row, col] == 1) return true;
            return false;
        }

        private void FunctionForLastClick(Button b)
        {
            //MessageBox.Show("First");
                //white ki chance hai                         par piece black ka hai
            if (whiteChance==true && pieces[row, col]!=0 && pieces[row, col] / 10 == 0 && lastClick==null) return;
            if (whiteChance==false && pieces[row, col] != 0 && pieces[row, col] / 10 == 1 && lastClick==null) return;
            //MessageBox.Show("first1");
            if (lastClick == null && b.BackgroundImage != null)
            {
               //MessageBox.Show("First Click");
               
                lastClickCol = col;
                lastClickRow = row;
                pictureBox1.Image = b.BackgroundImage;
                lastClick = b;
                PossibleMoves(lastClick);
            }
            else if(pieces[row,col]!=0 && pieces[lastClickRow, lastClickCol]/10== pieces[row, col]/10 && lastClick!=b)//for changing the piece of same side
            {
                //MessageBox.Show("Swap Piece");
                RemovePreviousPossibleMoves();
                lastClickCol = col;
                lastClickRow = row;
                lastClick = b;
                pictureBox1.Image = b.BackgroundImage;
                PossibleMoves(lastClick);
            }
            else if (lastClick != null && lastClick != b)
            {
               // MessageBox.Show("Move Piece");
                if (IsMovementPossible(b))
                {
                    // change color of king button background after king move
                    if (pieces[lastClickRow, lastClickCol] == 6 || pieces[lastClickRow, lastClickCol] == 16)
                    {
                        char g = color[lastClickRow, lastClickCol];
                        if (g == 'b') buttonArray[lastClickRow, lastClickCol].BackColor = buttonColor1;
                        else buttonArray[lastClickRow, lastClickCol].BackColor = buttonColor2; 
                    }
                    



                    RemovePreviousPossibleMoves();
                    if (pieces[lastClickRow, lastClickCol] == 6)
                    {
                        BlackKingColumnIndex = col;
                        BlackKingRowIndex = row;
                    }
                    if( pieces[lastClickRow, lastClickCol] == 16)
                    {
                        WhiteKingColumnIndex = col;
                        WhiteKingRowIndex = row;
                    }
                    if (whiteChance)
                    {
                        label1.Text = "Black Chance";
                        pictureBox1.Image = Properties.Resources.blackpawn1;
                        whiteChance = false;
                    }
                    else 
                    {
                        label1.Text = "White Chance";
                        pictureBox1.Image = Properties.Resources.whitepawn1;
                        whiteChance = true; 
                    }
                    ///////*********** Here we have to false all check booleans
                    if (checkFromBothSide || isRookCheck || isBishopCheck || isPawnCheck || isKnightCheck)
                    {
                        if (!whiteChance)
                        {
                            char g = color[WhiteKingRowIndex, WhiteKingColumnIndex];
                            if (g == 'b') buttonArray[WhiteKingRowIndex, WhiteKingColumnIndex].BackColor = buttonColor1;
                            else buttonArray[WhiteKingRowIndex, WhiteKingColumnIndex].BackColor = buttonColor2; ;

                        }
                        else
                        {
                            char g = color[BlackKingRowIndex, BlackKingColumnIndex];
                            if (g == 'b') buttonArray[BlackKingRowIndex, BlackKingColumnIndex].BackColor = buttonColor1;
                            else buttonArray[BlackKingRowIndex, BlackKingColumnIndex].BackColor = buttonColor2; ;

                        }


                        for (int i = 0; i < 8; i++)
                        {
                            for(int j = 0; j < 8; j++)
                            {
                                checkCoordinates[i, j] = 0;
                            }
                        }
                    }
                    
                    
                    
                    checkFromBothSide = false;
                    isPawnCheck = false;
                    isRookCheck = false;
                    isBishopCheck = false;
                    isKnightCheck = false;
                    count = 0;
                    pieces[row, col] = pieces[lastClickRow, lastClickCol];
                    pieces[lastClickRow,lastClickCol] = 0;
                    b.BackgroundImage = lastClick.BackgroundImage;
                    lastClick.BackgroundImage = null;
                    if (isIncrement == false)
                    {
                        IsCheck();
                        lastClick = null;
                    }
                    if (isRookCheck || isBishopCheck || isKnightCheck || isPawnCheck)
                    {
                        if (whiteChance)
                        {
                            buttonArray[WhiteKingRowIndex, WhiteKingColumnIndex].BackColor = Color.Red;
                        }
                        else
                        {
                            buttonArray[BlackKingRowIndex, BlackKingColumnIndex].BackColor = Color.Red;
                        }
                    }

                }
                else
                {
                    //MessageBox.Show("Hello");
                    //Color c = buttonArray[row, col].BackColor;
                    //buttonArray[row,col].BackColor = Color.Red;

                    //buttonArray[row, col].BackColor = c;


                }

            }
           
            /*else
            {
                MessageBox.Show("Last Else");
                if (lastClick == null) lastClick = null;
                else lastClick = b;
            }*/
            
            
        }


        public Form1()
        {
            

            InitializeComponent();
            Size = new Size(1200, 750);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            panel2.Visible = false;
            QueenPictureBox.Visible = false;
            RookPictureBox.Visible = false;
            BishopPictureBox.Visible = false;
            KnightPictureBox.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            button2.Enabled = false;

            
            //hathi
            pieces[0, 0] = 5;//b
            pieces[7, 0] = 15;//w

            //ghoda
            pieces[0, 1] = 9;
            pieces[7, 1] = 19;

            //unth
            pieces[0, 2] = 8;
            pieces[7, 2] = 18;

            //vazir
            pieces[0, 3] = 7;
            pieces[7, 3] = 17;

            //king
            pieces[0, 4] = 6;
            pieces[7, 4] = 16;

            //unth
            pieces[0, 5] = 8;
            pieces[7, 5] = 18;

            //ghoda
            pieces[0, 6] = 9;
            pieces[7, 6] = 19;

            //hathi
            pieces[0, 7] = 5;
            pieces[7, 7] = 15;

            
                
            for(int j = 0; j < 8; j++)
            {
                //pieces[5, j] = 1;
                pieces[6, j] = 11;
                pieces[1, j] = 1;
            }
            //Row 0
            buttonArray[0, 0] = A8;
            buttonArray[0, 1] = B8;
            buttonArray[0, 2] = C8;
            buttonArray[0, 3] = D8;
            buttonArray[0, 4] = E8;
            buttonArray[0, 5] = F8;
            buttonArray[0, 6] = G8;
            buttonArray[0, 7] = H8;

            //Row 1
            buttonArray[1, 0] = A7;
            buttonArray[1, 1] = B7;
            buttonArray[1, 2] = C7;
            buttonArray[1, 3] = D7;
            buttonArray[1, 4] = E7;
            buttonArray[1, 5] = F7;
            buttonArray[1, 6] = G7;
            buttonArray[1, 7] = H7;

            //Row 2
            buttonArray[2, 0] = A6;
            buttonArray[2, 1] = B6;
            buttonArray[2, 2] = C6;
            buttonArray[2, 3] = D6;
            buttonArray[2, 4] = E6;
            buttonArray[2, 5] = F6;
            buttonArray[2, 6] = G6;
            buttonArray[2, 7] = H6;

            //Row 3
            buttonArray[3, 0] = A5;
            buttonArray[3, 1] = B5;
            buttonArray[3, 2] = C5;
            buttonArray[3, 3] = D5;
            buttonArray[3, 4] = E5;
            buttonArray[3, 5] = F5;
            buttonArray[3, 6] = G5;
            buttonArray[3, 7] = H5;

            //Row 4
            buttonArray[4, 0] = A4;
            buttonArray[4, 1] = B4;
            buttonArray[4, 2] = C4;
            buttonArray[4, 3] = D4;
            buttonArray[4, 4] = E4;
            buttonArray[4, 5] = F4;
            buttonArray[4, 6] = G4;
            buttonArray[4, 7] = H4;

            //Row 5
            buttonArray[5, 0] = A3;
            buttonArray[5, 1] = B3;
            buttonArray[5, 2] = C3;
            buttonArray[5, 3] = D3;
            buttonArray[5, 4] = E3;
            buttonArray[5, 5] = F3;
            buttonArray[5, 6] = G3;
            buttonArray[5, 7] = H3;

            //Row 6
            buttonArray[6, 0] = A2;
            buttonArray[6, 1] = B2;
            buttonArray[6, 2] = C2;
            buttonArray[6, 3] = D2;
            buttonArray[6, 4] = E2;
            buttonArray[6, 5] = F2;
            buttonArray[6, 6] = G2;
            buttonArray[6, 7] = H2;

            //Row 7
            buttonArray[7, 0] = A1;
            buttonArray[7, 1] = B1;
            buttonArray[7, 2] = C1;
            buttonArray[7, 3] = D1;
            buttonArray[7, 4] = E1;
            buttonArray[7, 5] = F1;
            buttonArray[7, 6] = G1;
            buttonArray[7, 7] = H1;

            //for storing block color
            Color c1;
            Color c2;
            for(int i=0; i < 8; i++)
            {
                c1 = buttonColor1;
                c2 = buttonColor2;
                char firstColor = 'b';
                char secondColor = 'w';
                if (i % 2 == 0)
                {
                    c2 = buttonColor1;
                    c1 = buttonColor2;

                    firstColor = 'w';
                    secondColor = 'b';
                }
                for(int j=0; j < 8;)
                {
                    color[i,j] = firstColor;
                    buttonArray[i, j].BackColor = c1;
                    j++;
                    color[i,j] = secondColor;
                    buttonArray[i, j].BackColor = c2;

                    j++;
                }
            }

            whitePictureBox[0] = w1;
            whitePictureBox[1] = w2;
            whitePictureBox[2] = w3;
            whitePictureBox[3] = w4;
            whitePictureBox[4] = w5;
            whitePictureBox[5] = w6;
            whitePictureBox[6] = w7;
            whitePictureBox[7] = w8;
            whitePictureBox[8] = w9;
            whitePictureBox[9] = w10;
            whitePictureBox[10] = w11;
            whitePictureBox[11] = w12;
            whitePictureBox[12] = w13;
            whitePictureBox[13] = w14;
            whitePictureBox[14] = w15;

            blackPictureBox[0] = k1;
            blackPictureBox[1] = k2;
            blackPictureBox[2] = k3;
            blackPictureBox[3] = k4;
            blackPictureBox[4] = k5;
            blackPictureBox[5] = k6;
            blackPictureBox[6] = k7;
            blackPictureBox[7] = k8;
            blackPictureBox[8] = k9;
            blackPictureBox[9] = k10;
            blackPictureBox[10] =k11;
            blackPictureBox[11] =k12;
            blackPictureBox[12] =k13;
            blackPictureBox[13] =k14;
            blackPictureBox[14] =k15;



        }

        /*private void A1_Click(object sender, EventArgs e)
        {
           
                
        }

        private void E1_Click(object sender, EventArgs e)
        {
            
        }*/


        // new game button
        private void button1_Click_1(object sender, EventArgs e)
        {
            stack.ClearStack();
            

            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    char g = color[i,j];
                    if (g == 'b') buttonArray[i,j].BackColor = buttonColor1;
                    else buttonArray[i,j].BackColor = buttonColor2;
                }
            }


            panel1.Enabled = true;
            for (int i = 0; i < 8; i++)
            {

                for (int j = 0; j < 8; j++)
                {
                    buttonArray[i, j].BackgroundImage = null;
                    pieces[i, j] = 0;
                    coordinatesOfPossibleMoves[i, j] = 0;
                    checkCoordinates[i, j] = 0;
                }
            }
            for (int i = 0; i < whiteKilledCount; i++)
            {
                whitePictureBox[i].Image = null;
            }

            for (int i = 0; i < blackKilledCount; i++)
            {
                blackPictureBox[i].Image = null;
            }
            panel3.Visible=false;
            panel4.Visible=false;
            checkFromBothSide = false;
            isPawnCheck = false;
            isRookCheck = false;
            isBishopCheck = false;
            isKnightCheck = false;
            isIncrement = false;
            count = 0;
            lastClick = null;
            isLeftWhiteRookMoved = false;
            isRightWhiteRookMoved = false;
            isWhiteKingMoved = false;
            isLeftBlackRookMoved = false;
            isRightBlackRookMoved = false;
            isBlackKingMoved = false;


            

            count = 0;
            whiteKilledCount = 0;
            blackKilledCount = 0;

            WhiteKingRowIndex = 7;
             WhiteKingColumnIndex = 4;
           


            RookRowIndex = 0;
            RookColIndex = 0;
            BishopRowIndex = 0;
            BishopColIndex = 0;
            whiteChance = true;
            label1.Text = "White Chance";
            pictureBox1.Image = Properties.Resources.whitepawn1;


            BlackKingRowIndex = 0;
            BlackKingColumnIndex = 4;
            panel2.Visible = false;
            QueenPictureBox.Visible = false;
            RookPictureBox.Visible = false;
            BishopPictureBox.Visible = false;
            KnightPictureBox.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            

            //hathi
            pieces[0, 0] = 5;//b
            pieces[7, 0] = 15;//w
            buttonArray[0, 0].BackgroundImage = Properties.Resources.blackrook1;
            buttonArray[7, 0].BackgroundImage = Properties.Resources.whiterook;

            //ghoda
            pieces[0, 1] = 9;
            pieces[7, 1] = 19;
            buttonArray[0, 1].BackgroundImage = Properties.Resources.blackknight1;
            buttonArray[7, 1].BackgroundImage = Properties.Resources.whiteknight;

            //unth
            pieces[0, 2] = 8;
            pieces[7, 2] = 18;
            buttonArray[0, 2].BackgroundImage = Properties.Resources.blackbishop2;
            buttonArray[7, 2].BackgroundImage = Properties.Resources.whitebishop1;

            //vazir
            pieces[0, 3] = 7;
            pieces[7, 3] = 17;
            buttonArray[0, 3].BackgroundImage = Properties.Resources.blackqueen1;
            buttonArray[7, 3].BackgroundImage = Properties.Resources.whitequeen;


            //king
            pieces[0, 4] = 6;
            pieces[7, 4] = 16;
            buttonArray[0, 4].BackgroundImage = Properties.Resources.blackking1;
            buttonArray[7, 4].BackgroundImage = Properties.Resources.whiteking2;


            //unth
            pieces[0, 5] = 8;
            pieces[7, 5] = 18;
            buttonArray[0, 5].BackgroundImage = Properties.Resources.blackbishop2;
            buttonArray[7, 5].BackgroundImage = Properties.Resources.whitebishop1;


            //ghoda
            pieces[0, 6] = 9;
            pieces[7, 6] = 19;
            buttonArray[0, 6].BackgroundImage = Properties.Resources.blackknight1;
            buttonArray[7, 6].BackgroundImage = Properties.Resources.whiteknight;

            //hathi
            pieces[0, 7] = 5;
            pieces[7, 7] = 15;
            buttonArray[0, 7].BackgroundImage = Properties.Resources.blackrook1;
            buttonArray[7, 7].BackgroundImage = Properties.Resources.whiterook;



            for (int j = 0; j < 8; j++)
            {
                //pieces[5, j] = 1;
                pieces[6, j] = 11;
                pieces[1, j] = 1;
                buttonArray[1, j].BackgroundImage = Properties.Resources.blackpawn1;
                buttonArray[6, j].BackgroundImage = Properties.Resources.whitepawn1;

            }

        }
        private void button1_Click(object sender, EventArgs e)
        {//H8
            row = 0;
            col = 7;
            
            FunctionForLastClick(H8);
            
        }

        private void button2_Click(object sender, EventArgs e)//E8
        {
            row = 0;
            col = 4;
            FunctionForLastClick(E8);
        }

        private void button56_Click(object sender, EventArgs e)
        {//C4
            row = 4;
            col = 2;
            FunctionForLastClick(C4);
        }

        private void button55_Click(object sender, EventArgs e)
        {//B4
            row = 4;
            col = 1;
            FunctionForLastClick(B4);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void C8_Click(object sender, EventArgs e)
        {
            row = 0;
            col = 2;
            FunctionForLastClick(C8);
        }

        private void A1_Click_1(object sender, EventArgs e)
        {
            row = 7;
            col = 0;
            FunctionForLastClick(A1);
            
        }

        private void D1_Click(object sender, EventArgs e)
        {
            row = 7;
            col = 3;
            FunctionForLastClick(D1);
        }

        private void A8_Click(object sender, EventArgs e)
        {
            row = 0;
            col = 0;
            FunctionForLastClick(A8);
        }

        private void B8_Click(object sender, EventArgs e)
        {
            row = 0;
            col = 1;
            FunctionForLastClick(B8);
        }

        private void D8_Click(object sender, EventArgs e)
        {
            row = 0;
            col = 3;
            FunctionForLastClick(D8);
        }

        private void F8_Click(object sender, EventArgs e)
        {
            row = 0;
            col = 5;
            FunctionForLastClick(F8);
        }

        private void G8_Click(object sender, EventArgs e)
        {
            row = 0;
            col = 6;
            FunctionForLastClick(G8);
        }

        private void A7_Click(object sender, EventArgs e)
        {
            row = 1;
            col = 0;
            FunctionForLastClick(A7);
        }

        private void B7_Click(object sender, EventArgs e)
        {
            row = 1;
            col = 1;
            FunctionForLastClick(B7);
        }

        private void C7_Click(object sender, EventArgs e)
        {
            row = 1;
            col = 2;
            FunctionForLastClick(C7);
        }

        private void D7_Click(object sender, EventArgs e)
        {
            row = 1;
            col = 3;
            FunctionForLastClick(D7);
        }

        private void E7_Click(object sender, EventArgs e)
        {
            row = 1;
            col = 4;
            FunctionForLastClick(E7);
        }

        private void F7_Click(object sender, EventArgs e)
        {
            row = 1;
            col = 5;
            FunctionForLastClick(F7);
        }

        private void G7_Click(object sender, EventArgs e)
        {
            row = 1;
            col = 6;
            FunctionForLastClick(G7);
        }

        private void H7_Click(object sender, EventArgs e)
        {
            row = 1;
            col = 7;
            FunctionForLastClick(H7);
        }

        private void A6_Click(object sender, EventArgs e)
        {
            row = 2;
            col = 0;
            FunctionForLastClick(A6);
        }

        private void B6_Click(object sender, EventArgs e)
        {
            row = 2;
            col = 1;
            FunctionForLastClick(B6);
        }

        private void C6_Click(object sender, EventArgs e)
        {
            row = 2;
            col = 2;
            FunctionForLastClick(C6);
        }

        private void D6_Click(object sender, EventArgs e)
        {
            row = 2;
            col = 3;
            FunctionForLastClick(D6);
        }

        private void E6_Click(object sender, EventArgs e)
        {
            row = 2;
            col = 4;
            FunctionForLastClick(E6);
        }

        private void F6_Click(object sender, EventArgs e)
        {
            row = 2;
            col = 5;
            FunctionForLastClick(F6);
        }

        private void G6_Click(object sender, EventArgs e)
        {
            row = 2;
            col = 6;
            FunctionForLastClick(G6);
        }

        private void H6_Click(object sender, EventArgs e)
        {
            row = 2;
            col = 7;
            FunctionForLastClick(H6);
        }

        private void A5_Click(object sender, EventArgs e)
        {
            row = 3;
            col = 0;
            FunctionForLastClick(A5);
        }

        private void B5_Click(object sender, EventArgs e)
        {
            row = 3;
            col = 1;
            FunctionForLastClick(B5);
        }

        private void C5_Click(object sender, EventArgs e)
        {
            row = 3;
            col = 2;
            FunctionForLastClick(C5);
        }

        private void D5_Click(object sender, EventArgs e)
        {
            row = 3;
            col = 3;
            FunctionForLastClick(D5);
        }

        private void E5_Click(object sender, EventArgs e)
        {
            row = 3;
            col = 4;
            FunctionForLastClick(E5);
        }

        private void F5_Click(object sender, EventArgs e)
        {
            row = 3;
            col = 5;
            FunctionForLastClick(F5);
        }

        private void G5_Click(object sender, EventArgs e)
        {
            row = 3;
            col = 6;
            FunctionForLastClick(G5);
        }

        private void H5_Click(object sender, EventArgs e)
        {
            row = 3;
            col = 7;
            FunctionForLastClick(H5);
        }

        private void A4_Click(object sender, EventArgs e)
        {
            row = 4;
            col = 0;
            FunctionForLastClick(A4);
        }

        private void D4_Click(object sender, EventArgs e)
        {
            row = 4;
            col = 3;
            FunctionForLastClick(D4);
        }

        private void E4_Click(object sender, EventArgs e)
        {
            row = 4;
            col = 4;
            FunctionForLastClick(E4);
        }

        private void F4_Click(object sender, EventArgs e)
        {
            row = 4;
            col = 5;
            FunctionForLastClick(F4);
        }

        private void G4_Click(object sender, EventArgs e)
        {
            row = 4;
            col = 6;
            FunctionForLastClick(G4);
        }

        private void H4_Click(object sender, EventArgs e)
        {
            row = 4;
            col = 7;
            FunctionForLastClick(H4);
        }

        private void A3_Click(object sender, EventArgs e)
        {
            row = 5;
            col = 0;
            FunctionForLastClick(A3);
        }

        private void B3_Click(object sender, EventArgs e)
        {
            row = 5;
            col = 1;
            FunctionForLastClick(B3);
        }

        private void C3_Click(object sender, EventArgs e)
        {
            row = 5;
            col = 2;
            FunctionForLastClick(C3);
        }

        private void D3_Click(object sender, EventArgs e)
        {
            row = 5;
            col = 3;
            FunctionForLastClick(D3);
        }

        private void E3_Click(object sender, EventArgs e)
        {
            row = 5;
            col = 4;
            FunctionForLastClick(E3);
        }

        private void F3_Click(object sender, EventArgs e)
        {
            row = 5;
            col = 5;
            FunctionForLastClick(F3);
        }

        private void G3_Click(object sender, EventArgs e)
        {
            row = 5;
            col = 6;
            FunctionForLastClick(G3);
        }

        private void H3_Click(object sender, EventArgs e)
        {
            row = 5;
            col = 7;
            FunctionForLastClick(H3);
        }

        private void A2_Click(object sender, EventArgs e)
        {
            row = 6;
            col = 0;
            FunctionForLastClick(A2);
        }

        private void B2_Click(object sender, EventArgs e)
        {
            row = 6;
            col = 1;
            FunctionForLastClick(B2);
        }

        private void C2_Click(object sender, EventArgs e)
        {
            row = 6;
            col = 2;
            FunctionForLastClick(C2);
        }

        private void D2_Click(object sender, EventArgs e)
        {
            row = 6;
            col = 3;
            FunctionForLastClick(D2);
        }

        private void E2_Click(object sender, EventArgs e)
        {
            row = 6;
            col = 4;
            FunctionForLastClick(E2);
        }

        private void F2_Click(object sender, EventArgs e)
        {
            row = 6;
            col = 5;
            FunctionForLastClick(F2);
        }

        private void G2_Click(object sender, EventArgs e)
        {
            row = 6;
            col = 6;
            FunctionForLastClick(G2);
        }

        private void H2_Click(object sender, EventArgs e)
        {
            row = 6;
            col = 7;
            FunctionForLastClick(H2);
        }

        private void B1_Click(object sender, EventArgs e)
        {
            row = 7;
            col = 1;
            FunctionForLastClick(B1);
        }

        private void C1_Click(object sender, EventArgs e)
        {
            row = 7;
            col = 2;
            FunctionForLastClick(C1);
        }

        private void E1_Click_1(object sender, EventArgs e)
        {
            row = 7;
            col = 4;
            FunctionForLastClick(E1);

        }

        private void F1_Click(object sender, EventArgs e)
        {
            row = 7;
            col = 5;
            FunctionForLastClick(F1);
        }

        private void QueenPictureBox_Click(object sender, EventArgs e)
        {
            if (!whiteChance)
            {
                pieces[row, col] = 17;
                buttonArray[row ,col].BackgroundImage = Properties.Resources.whitequeen;
                for (int i = 0; i < whiteKilledCount; i++)
                {
                    if (whiteKilledPiecesNumber[i] == 17)
                    { 
                    whitePictureBox[i].Image = Properties.Resources.whitepawn1;
                    }
                }
                
            }
            else
            {
                pieces[row, col] = 7;
                buttonArray[row, col].BackgroundImage = Properties.Resources.blackqueen;
                for (int i = 0; i < blackKilledCount; i++)
                {
                    if (blackKilledPiecesNumber[i] == 7)
                    {
                        blackPictureBox[i].Image = Properties.Resources.blackpawn1;
                    }
                }
            }
            panel2.Visible = false;
            QueenPictureBox.Visible = false;
            RookPictureBox.Visible = false;
            BishopPictureBox.Visible = false;
            KnightPictureBox.Visible = false;
            IsCheck();
            if (isRookCheck || isBishopCheck || isKnightCheck || isPawnCheck || isKnightCheck)
            {
                if (whiteChance)
                {
                    buttonArray[WhiteKingRowIndex, WhiteKingColumnIndex].BackColor = Color.Red;
                }
                else
                {
                    buttonArray[BlackKingRowIndex, BlackKingColumnIndex].BackColor = Color.Red;
                }
            }
            lastClick = null;
            isIncrement = false;
        }

        private void RookPictureBox_Click(object sender, EventArgs e)
        {
            if (!whiteChance)
            {
                pieces[row, col] = 15;
                buttonArray[row, col].BackgroundImage = Properties.Resources.whiterook;
                for (int i = 0; i < whiteKilledCount; i++)
                {
                    if (whiteKilledPiecesNumber[i] == 15)
                    {
                        whitePictureBox[i].Image = Properties.Resources.whitepawn1;
                    }
                }

            }
            else
            {
                pieces[row, col] = 5;
                buttonArray[row, col].BackgroundImage = Properties.Resources.blackrook;
                for (int i = 0; i < blackKilledCount; i++)
                {
                    if (blackKilledPiecesNumber[i] == 5)
                    {
                        blackPictureBox[i].Image = Properties.Resources.blackpawn1;
                    }
                }
            }
            panel2.Visible = false;
            QueenPictureBox.Visible = false;
            RookPictureBox.Visible = false;
            BishopPictureBox.Visible = false;
            KnightPictureBox.Visible = false;
            IsCheck();
            if (isRookCheck || isBishopCheck || isKnightCheck || isPawnCheck || isKnightCheck)
            {
                if (whiteChance)
                {
                    buttonArray[WhiteKingRowIndex, WhiteKingColumnIndex].BackColor = Color.Red;
                }
                else
                {
                    buttonArray[BlackKingRowIndex, BlackKingColumnIndex].BackColor = Color.Red;
                }
            }
            lastClick = null;
            isIncrement = false;
        }

        private void KnightPictureBox_Click(object sender, EventArgs e)
        {
            if (!whiteChance)
            {
                pieces[row, col] = 19;
                buttonArray[row, col].BackgroundImage = Properties.Resources.whiteknight;
                for (int i = 0; i < whiteKilledCount; i++)
                {
                    if (whiteKilledPiecesNumber[i] == 19)
                    {
                        whitePictureBox[i].Image = Properties.Resources.whitepawn1;
                    }
                }
            }
            else
            {
                pieces[row, col] = 9;
                buttonArray[row, col].BackgroundImage = Properties.Resources.blackknight;
                for (int i = 0; i < blackKilledCount; i++)
                {
                    if (blackKilledPiecesNumber[i] == 9)
                    {
                        blackPictureBox[i].Image = Properties.Resources.blackpawn1;
                    }
                }
            }
            panel2.Visible = false;
            QueenPictureBox.Visible = false;
            RookPictureBox.Visible = false;
            BishopPictureBox.Visible = false;
            KnightPictureBox.Visible = false;
            IsCheck();
            if (isRookCheck || isBishopCheck || isKnightCheck || isPawnCheck || isKnightCheck)
            {
                if (whiteChance)
                {
                    buttonArray[WhiteKingRowIndex, WhiteKingColumnIndex].BackColor = Color.Red;
                }
                else
                {
                    buttonArray[BlackKingRowIndex, BlackKingColumnIndex].BackColor = Color.Red;
                }
            }
            lastClick = null;
            isIncrement = false;
        }

        private void BishopPictureBox_Click(object sender, EventArgs e)
        {
            if (!whiteChance)
            {
                pieces[row, col] = 18;
                buttonArray[row, col].BackgroundImage = Properties.Resources.whitebishop;
                for (int i = 0; i < whiteKilledCount; i++)
                {
                    if (whiteKilledPiecesNumber[i] == 18)
                    {
                        whitePictureBox[i].Image = Properties.Resources.whitepawn1;
                    }
                }

            }
            else
            {
                pieces[row, col] = 8;
                buttonArray[row, col].BackgroundImage = Properties.Resources.blackbishop;
                for (int i = 0; i < blackKilledCount; i++)
                {
                    if (blackKilledPiecesNumber[i] == 8)
                    {
                        blackPictureBox[i].Image = Properties.Resources.blackpawn1;
                    }
                }

            }
            panel2.Visible = false;
            QueenPictureBox.Visible = false;
            RookPictureBox.Visible = false;
            BishopPictureBox.Visible = false;
            KnightPictureBox.Visible = false;
            IsCheck();
            if (isRookCheck || isBishopCheck || isKnightCheck || isPawnCheck || isKnightCheck)
            {
                if (whiteChance)
                {
                    buttonArray[WhiteKingRowIndex, WhiteKingColumnIndex].BackColor = Color.Red;
                }
                else
                {
                    buttonArray[BlackKingRowIndex, BlackKingColumnIndex].BackColor = Color.Red;
                }
            }
            lastClick = null;
            isIncrement = false;
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {//undo button
            
            PiecesData pd = stack.Pop();
            if (pd != null)
            {
                if (isBishopCheck) isBishopCheck = false;
                if (isKnightCheck) isKnightCheck = false;
                if (isRookCheck) isRookCheck = false;
                if (isPawnCheck) isPawnCheck = false;
                
                pieces[pd.initialRowIndex, pd.initialColumnIndex] = pd.pieceNumber;
                pieces[pd.currentRowIndex, pd.currentColumnIndex] = pd.previousPieceNumber;
                buttonArray[pd.initialRowIndex, pd.initialColumnIndex].BackgroundImage = pd.currentImage;
                RemovePath();
                if (pd.previousPieceNumber != 0)
                {
                    buttonArray[pd.currentRowIndex, pd.currentColumnIndex].BackgroundImage = pd.previousImage;
                    char g = color[pd.currentRowIndex, pd.currentColumnIndex];
                    if (g == 'b') buttonArray[pd.currentRowIndex, pd.currentColumnIndex].BackColor = buttonColor1;
                    else buttonArray[pd.currentRowIndex, pd.currentColumnIndex].BackColor = buttonColor2;;
                    
                    // killed pieces panel me se hatane k liye
                    if (pd.previousPieceNumber / 10 == 1 && whiteKilledCount != 0)
                    {
                        whiteKilledCount--;
                        whitePictureBox[whiteKilledCount].Image = null;

                        if (whiteKilledCount == 0)
                        {
                            panel3.Visible = false;
                        }
                    }
                    else if (pd.previousPieceNumber / 10 == 0 && blackKilledCount != 0)
                    {
                        blackKilledCount--;
                        blackPictureBox[blackKilledCount].Image = null;

                        if (blackKilledCount == 0)
                        {
                            panel4.Visible = false;
                        }
                    }
                }
                else
                {
                    if(pd.pieceNumber==15 && pd.whiteLeftRookMoved == true)
                    {
                        isLeftWhiteRookMoved = false;
                    }
                    if (pd.pieceNumber == 15 && pd.whiteRightRookMoved == true)
                    {
                        isRightWhiteRookMoved = false;
                    }
                    if (pd.pieceNumber == 5 && pd.blackRightRookMoved == true)
                    {
                        isRightBlackRookMoved = false;
                    }
                    if (pd.pieceNumber == 5 && pd.blackLeftRookMoved == true)
                    {
                        isLeftBlackRookMoved = false;
                    }
                    // castling hui thi
                    if (pd.pieceNumber == 16)
                    {// white king
                        whiteKingMoveCount--;
                        if(whiteKingMoveCount == 0)
                        {
                            isWhiteKingMoved = false;
                        }


                        if (pd.currentColumnIndex - pd.initialColumnIndex == (2))
                        {// right side castling 
                            buttonArray[7, 7].BackgroundImage = Properties.Resources.whiterook;
                            buttonArray[7, 5].BackgroundImage = null;
                            pieces[7, 7] = 15;
                            pieces[7, 5] = 0;
                            WhiteKingColumnIndex = 4;
                            isRightWhiteRookMoved = false;

                        }
                        else if (pd.currentColumnIndex - pd.initialColumnIndex == (-2))
                        {// left side castling 
                            buttonArray[7, 0].BackgroundImage = Properties.Resources.whiterook;
                            buttonArray[7, 3].BackgroundImage = null;
                            pieces[7, 0] = 15;
                            pieces[7, 3] = 0;
                            WhiteKingColumnIndex = 4;
                            isLeftWhiteRookMoved = false;
                        }
                        
                        isWhiteKingMoved = false;
                    }
                    else if (pd.pieceNumber == 6)
                    { //black king

                        blackKingMoveCount--;
                        if (blackKingMoveCount == 0)
                        {
                            isBlackKingMoved = false;
                        }

                        if (pd.currentColumnIndex - pd.initialColumnIndex == 2)
                        {// right side castling 
                            buttonArray[0, 7].BackgroundImage = Properties.Resources.blackrook1;
                            buttonArray[0, 5].BackgroundImage = null;
                            pieces[0, 7] = 5;
                            pieces[0, 5] = 0;
                            isRightBlackRookMoved = false;
                            BlackKingColumnIndex = 4;
                            isBlackKingMoved = false;
                        }
                        else if (pd.currentColumnIndex - pd.initialColumnIndex == (-2))
                        {// left side castling 
                            buttonArray[0, 0].BackgroundImage = Properties.Resources.blackrook1;
                            buttonArray[0, 3].BackgroundImage = null;
                            pieces[0, 0] = 5;
                            pieces[0, 3] = 0;
                            BlackKingColumnIndex = 4;
                            isLeftBlackRookMoved = false;
                            isBlackKingMoved = false;
                        }
                       
                        
                    }
                    buttonArray[pd.currentRowIndex, pd.currentColumnIndex].BackgroundImage = null;
                    char g = color[pd.currentRowIndex, pd.currentColumnIndex];
                    if (g == 'b') buttonArray[pd.currentRowIndex, pd.currentColumnIndex].BackColor = buttonColor1;
                    else buttonArray[pd.currentRowIndex, pd.currentColumnIndex].BackColor = buttonColor2;;

                }
                if (pd.pieceNumber / 10 == 0)
                {
                    label1.Text = "Black Chance";//done done
                    pictureBox1.Image = Properties.Resources.blackpawn1;
                    whiteChance = false;
                }
                if (pd.pieceNumber / 10 == 1)
                {
                    label1.Text = "White Chance";
                    pictureBox1.Image = Properties.Resources.whitepawn1;
                    whiteChance = true;
                }
                lastClick = null;
               
                    if (!whiteChance)
                    {
                        char g = color[WhiteKingRowIndex, WhiteKingColumnIndex];
                        if (g == 'b') buttonArray[WhiteKingRowIndex, WhiteKingColumnIndex].BackColor = buttonColor1;
                        else buttonArray[WhiteKingRowIndex, WhiteKingColumnIndex].BackColor = buttonColor2;;

                    }
                    else
                    {
                        char g = color[BlackKingRowIndex, BlackKingColumnIndex];
                        if (g == 'b') buttonArray[BlackKingRowIndex, BlackKingColumnIndex].BackColor = buttonColor1;
                        else buttonArray[BlackKingRowIndex, BlackKingColumnIndex].BackColor = buttonColor2;;

                    }
                if (pd.pieceNumber == 16)
                {
                    WhiteKingRowIndex = pd.initialRowIndex;
                    WhiteKingColumnIndex = pd.initialColumnIndex;
                }
                else if (pd.pieceNumber == 6)
                {
                    BlackKingRowIndex = pd.initialRowIndex;
                    BlackKingColumnIndex = pd.initialColumnIndex;

                }

                IsCheck();
                if (isRookCheck || isBishopCheck || isKnightCheck || isPawnCheck)
                {
                    if (whiteChance)
                    {
                        buttonArray[WhiteKingRowIndex, WhiteKingColumnIndex].BackColor = Color.Red;
                    }
                    else
                    {
                        buttonArray[BlackKingRowIndex, BlackKingColumnIndex].BackColor = Color.Red;
                    }
                }
                
            }
            else
            {
                button2.Enabled = false;
            }
            

        }

        private void greenTheme_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) // green theme button
        {
            panel3.BackColor = Color.FromArgb(123, 164, 90);
            panel4.BackColor = Color.FromArgb(123, 164, 90);
            button1.BackColor= Color.FromArgb(123, 164, 90);
            button2.BackColor= Color.FromArgb(123, 164, 90);
            panel1.BackgroundImage = Properties.Resources.ChessBoardNew;
            buttonColor1 = Color.FromArgb(123, 164, 90);
            Color c1;
            Color c2;
            for (int i = 0; i < 8; i++)
            {
                c1 = buttonColor1;
                c2 = buttonColor2;
                char firstColor = 'b';
                char secondColor = 'w';
                if (i % 2 == 0)
                {
                    c2 = buttonColor1;
                    c1 = buttonColor2;

                    firstColor = 'w';
                    secondColor = 'b';
                }
                for (int j = 0; j < 8;)
                {
                    color[i, j] = firstColor;
                    buttonArray[i, j].BackColor = c1;
                    j++;
                    color[i, j] = secondColor;
                    buttonArray[i, j].BackColor = c2;

                    j++;
                }
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            panel3.BackColor = Color.FromArgb(107, 186, 227);
            panel4.BackColor = Color.FromArgb(107, 186, 227);
            button1.BackColor = Color.FromArgb(107, 186, 227);
            button2.BackColor = Color.FromArgb(107, 186, 227);
            panel1.BackgroundImage = Properties.Resources.blueChessBoard;
            buttonColor1 = Color.FromArgb(107, 186, 227);
            Color c1;
            Color c2;
            for (int i = 0; i < 8; i++)
            {
                c1 = buttonColor1;
                c2 = buttonColor2;
                char firstColor = 'b';
                char secondColor = 'w';
                if (i % 2 == 0)
                {
                    c2 = buttonColor1;
                    c1 = buttonColor2;

                    firstColor = 'w';
                    secondColor = 'b';
                }
                for (int j = 0; j < 8;)
                {
                    color[i, j] = firstColor;
                    buttonArray[i, j].BackColor = c1;
                    j++;
                    color[i, j] = secondColor;
                    buttonArray[i, j].BackColor = c2;

                    j++;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel3.BackColor = Color.FromArgb(249, 187, 230);
            panel4.BackColor = Color.FromArgb(249, 187, 230);
            button1.BackColor = Color.FromArgb(249, 187, 230);
            button2.BackColor = Color.FromArgb(249, 187, 230);
            panel1.BackgroundImage = Properties.Resources.lightPinkChessBoard;
            buttonColor1 = Color.FromArgb(249, 187, 230);
            Color c1;
            Color c2;
            for (int i = 0; i < 8; i++)
            {
                c1 = buttonColor1;
                c2 = buttonColor2;
                char firstColor = 'b';
                char secondColor = 'w';
                if (i % 2 == 0)
                {
                    c2 = buttonColor1;
                    c1 = buttonColor2;

                    firstColor = 'w';
                    secondColor = 'b';
                }
                for (int j = 0; j < 8;)
                {
                    color[i, j] = firstColor;
                    buttonArray[i, j].BackColor = c1;
                    j++;
                    color[i, j] = secondColor;
                    buttonArray[i, j].BackColor = c2;

                    j++;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel3.BackColor = Color.FromArgb(255, 127,80);
            panel4.BackColor = Color.FromArgb(255, 127, 80);
            button1.BackColor = Color.FromArgb(255, 127, 80);
            button2.BackColor = Color.FromArgb(255, 127, 80);
            panel1.BackgroundImage = Properties.Resources.orangeChessBoard;
            buttonColor1 = Color.FromArgb(255, 127, 80);
            Color c1;
            Color c2;
            for (int i = 0; i < 8; i++)
            {
                c1 = buttonColor1;
                c2 = buttonColor2;
                char firstColor = 'b';
                char secondColor = 'w';
                if (i % 2 == 0)
                {
                    c2 = buttonColor1;
                    c1 = buttonColor2;

                    firstColor = 'w';
                    secondColor = 'b';
                }
                for (int j = 0; j < 8;)
                {
                    color[i, j] = firstColor;
                    buttonArray[i, j].BackColor = c1;
                    j++;
                    color[i, j] = secondColor;
                    buttonArray[i, j].BackColor = c2;

                    j++;
                }
            }
        }

        private void G1_Click(object sender, EventArgs e)
        {
            row = 7;
            col = 6;
            FunctionForLastClick(G1);
        }

        private void H1_Click(object sender, EventArgs e)
        {
            row = 7;
            col = 7;
            FunctionForLastClick(H1);
        }
    }
}
