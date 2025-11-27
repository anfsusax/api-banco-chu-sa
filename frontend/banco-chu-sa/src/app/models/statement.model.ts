export interface Statement {
  accountId: string;
  accountNumber: string;
  startDate: string;
  endDate: string;
  initialBalance: number;
  finalBalance: number;
  transactions: TransactionItem[];
}

export interface TransactionItem {
  transactionDate: string;
  type: string;
  amount: number;
  description: string;
  relatedAccountNumber?: string;
}

