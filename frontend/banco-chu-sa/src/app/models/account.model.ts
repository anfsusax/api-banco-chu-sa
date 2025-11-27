export interface Account {
  id: string;
  accountNumber: string;
  ownerName: string;
  documentNumber: string;
  balance: number;
  createdAt: string;
}

export interface CreateAccount {
  ownerName: string;
  documentNumber: string;
  initialBalance: number;
}

