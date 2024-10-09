export interface Purchase {
  id: number;
  productId: number;
  userId: number;
  quantity: number;
  unitPrice: number;
  orderDate: Date;
  productName: string;
  userName?: string;
}
