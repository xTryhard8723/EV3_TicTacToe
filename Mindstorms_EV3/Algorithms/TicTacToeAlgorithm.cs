﻿using Mindstorms_EV3.EV3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mindstorms_EV3.Algorithms
{
    public class TicTacToeAlgorithm
    {
        private char[,] board = EV3Controls.getBoard();

        private char player = 'X';
        private char computer = 'O';




        public int[] ev3Move()
        {
            int[] move = getBestMove();
            board[move[0], move[1]] = computer;
            EV3Controls.setBoard(board);
            return move;
        }

        private int[] getBestMove()
        {
            int[] bestMove = { -1, -1 };
            int bestScore = int.MinValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = computer;
                        int score = minimax(board, 0, false);
                        board[i, j] = ' ';

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove[0] = i;
                            bestMove[1] = j;
                        }
                    }
                }
            }

            return bestMove;
        }


        private int minimax(char[,] currentBoard, int depth, bool isMaximizing)
        {
            char result = checkWinner();
            if (result != ' ')
            {
                return result == computer ? 1 : -1;
            }

            if (isBoardFull())
            {
                return 0;
            }

            int bestScore = isMaximizing ? int.MinValue : int.MaxValue;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (currentBoard[i, j] == ' ')
                    {
                        currentBoard[i, j] = isMaximizing ? computer : player;
                        int score = minimax(currentBoard, depth + 1, !isMaximizing);
                        currentBoard[i, j] = ' ';

                        if (isMaximizing)
                            bestScore = Math.Max(score, bestScore);
                        else
                            bestScore = Math.Min(score, bestScore);
                    }
                }
            }

            return bestScore;
        }

        public char checkWinner()
        {
            // Check rows, columns, and diagonals
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != ' ')
                    return board[i, 0];

                if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[0, i] != ' ')
                    return board[0, i];
            }

            if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != ' ')
                return board[0, 0];

            if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != ' ')
                return board[0, 2];

            return ' ';
        }

        public bool isBoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                        return false;
                }
            }
            return true;
        }


    }
}