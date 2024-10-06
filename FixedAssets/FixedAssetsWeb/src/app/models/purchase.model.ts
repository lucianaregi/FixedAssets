export interface Purchase {
  id: number;             
  productId: number;       
  userId: number;          
  quantity: number;        
  totalPrice: number;      
  purchaseDate: Date;      
  productName: string;     
  userName?: string;       
}
